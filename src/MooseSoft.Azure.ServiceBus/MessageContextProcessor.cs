using Microsoft.Azure.ServiceBus;
using Moosesoft.Azure.ServiceBus.Abstractions;
using Moosesoft.Azure.ServiceBus.FailurePolicy;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Moosesoft.Azure.ServiceBus
{
    internal class MessageContextProcessor : IMessageContextProcessor
    {
        private readonly IFailurePolicy _failurePolicy;
        private readonly IMessageProcessor _messageProcessor;
        private readonly Func<Exception, bool> _shouldComplete;

        public string Name { get; }

        public MessageContextProcessor(
            IMessageProcessor messageProcessor,
            IFailurePolicy failurePolicy = null,
            Func<Exception, bool> shouldComplete = null,
            string name = "default")
        {
            _messageProcessor = messageProcessor ?? throw new ArgumentNullException(nameof(messageProcessor));
            _failurePolicy = failurePolicy ?? new AbandonMessageFailurePolicy();
            _shouldComplete = shouldComplete;
            Name = name;
        }

        public MessageContextProcessor(
            Func<Message, CancellationToken, Task> processMessage,
            IFailurePolicy failurePolicy = null,
            Func<Exception, bool> shouldComplete = null)
            : this(new DefaultMessageProcessor(processMessage), failurePolicy, shouldComplete)
        {
        }

        public async Task ProcessMessageContextAsync(MessageContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                await CheckForDeferredMessageAsync(context).ConfigureAwait(false);

                await _messageProcessor.ProcessMessageAsync(context.Message, cancellationToken).ConfigureAwait(false);

                await context.MessageReceiver.CompleteAsync(context.Message.SystemProperties.LockToken)
                    .ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                if (await TryCompleteOnExceptionAsync(context, exception) || await TryAbandonOnExceptionAsync(context, exception))
                    return;

                await _failurePolicy.HandleFailureAsync(context, cancellationToken).ConfigureAwait(false);
            }
        }

        private static async Task CheckForDeferredMessageAsync(MessageContext context)
        {
            if (!context.Message.IsDeferredMessageLocator()) return;

            context.Message = await context.MessageReceiver.GetDeferredMessageAsync(context.Message);
        }

        private async Task<bool> TryCompleteOnExceptionAsync(MessageContext context, Exception exception)
        {
            if (_shouldComplete == null || !_shouldComplete(exception)) return false;

            await context.MessageReceiver.CompleteAsync(context.Message.SystemProperties.LockToken)
                .ConfigureAwait(false);

            return true;
        }

        private async Task<bool> TryAbandonOnExceptionAsync(MessageContext context, Exception exception)
        {
            if (_failurePolicy.CanHandle(exception)) return false;

            await context.MessageReceiver.AbandonAsync(context.Message.SystemProperties.LockToken)
                .ConfigureAwait(false);

            return true;
        }
    }
}
