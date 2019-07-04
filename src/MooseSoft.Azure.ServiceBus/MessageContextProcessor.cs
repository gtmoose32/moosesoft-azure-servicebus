using MooseSoft.Azure.ServiceBus.Abstractions;
using MooseSoft.Azure.ServiceBus.FailurePolicy;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace MooseSoft.Azure.ServiceBus
{
    /// <inheritdoc cref="IMessageContextProcessor"/>
    public class MessageContextProcessor : IMessageContextProcessor
    {
        private readonly IMessageProcessor _messageProcessor;
        private readonly IFailurePolicy _failurePolicy;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageProcessor"></param>
        /// <param name="failurePolicy"></param>
        public MessageContextProcessor(IMessageProcessor messageProcessor, IFailurePolicy failurePolicy = null)
        {
            _messageProcessor = messageProcessor ?? throw new ArgumentNullException(nameof(messageProcessor));
            _failurePolicy = failurePolicy ?? new AbandonMessageFailurePolicy();
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
                if (!_failurePolicy.CanHandle(exception))
                {
                    await context.MessageReceiver.AbandonAsync(context.Message.SystemProperties.LockToken)
                        .ConfigureAwait(false);

                    return;
                }

                await _failurePolicy.HandleFailureAsync(context, cancellationToken).ConfigureAwait(false);
            }           
        }

        private static async Task CheckForDeferredMessageAsync(MessageContext context)
        {
            var sequenceNumber = context.Message.GetDeferredSequenceNumber();
            if (sequenceNumber.HasValue)
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    await context.MessageReceiver.CompleteAsync(context.Message.SystemProperties.LockToken).ConfigureAwait(false);
                    context.Message = await context.MessageReceiver.ReceiveDeferredMessageAsync(sequenceNumber.Value)
                        .ConfigureAwait(false); 

                    scope.Complete();
                }
            }
        }
    }
}