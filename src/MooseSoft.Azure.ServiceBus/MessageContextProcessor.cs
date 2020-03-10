using MooseSoft.Azure.ServiceBus.Abstractions;
using MooseSoft.Azure.ServiceBus.FailurePolicy;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MooseSoft.Azure.ServiceBus
{
    /// <inheritdoc cref="IMessageContextProcessor"/>
    public class MessageContextProcessor : IMessageContextProcessor
    {
        private readonly IMessageProcessor _messageProcessor;
        private readonly IFailurePolicy _failurePolicy;
        private readonly Func<Exception, bool> _shouldComplete;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageProcessor"></param>
        /// <param name="failurePolicy"></param>
        /// <param name="shouldComplete"></param>
        public MessageContextProcessor(
            IMessageProcessor messageProcessor, 
            IFailurePolicy failurePolicy = null, 
            Func<Exception, bool> shouldComplete = null)
        {
            _messageProcessor = messageProcessor ?? throw new ArgumentNullException(nameof(messageProcessor));
            _failurePolicy = failurePolicy ?? new AbandonMessageFailurePolicy();
            _shouldComplete = shouldComplete;

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
            if (context.MessageReceiver.RegisteredPlugins.Any(p => p.Name == nameof(DeferredMessagePlugin))) return;

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