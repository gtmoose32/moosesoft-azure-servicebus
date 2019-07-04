using Microsoft.Azure.ServiceBus;
using MooseSoft.Azure.ServiceBus.Abstractions;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace MooseSoft.Azure.ServiceBus.FailurePolicy
{
    public class DeferMessageFailurePolicy : FailurePolicyBase
    {
        #region ctor
        public DeferMessageFailurePolicy(Func<Exception, bool> canHandle, int maxDeliveryCount, IBackOffDelayStrategy backOffDelayStrategy = null) 
            : base(canHandle, maxDeliveryCount, backOffDelayStrategy)
        {
        } 
        #endregion

        public override async Task HandleFailureAsync(MessageContext context, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (context.Message.SystemProperties.DeliveryCount >= MaxDeliveryCount)
            {
                await context.MessageReceiver.AbandonAsync(context.Message.SystemProperties.LockToken).ConfigureAwait(false);
                return;
            }

            var deferredPointer = CreateDeferredPointerMessage(context.Message);

            var sender = context.CreateMessageSender();

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await sender.SendAsync(deferredPointer).ConfigureAwait(false);
                await context.MessageReceiver.DeferAsync(context.Message.SystemProperties.LockToken).ConfigureAwait(false);
                scope.Complete();
            }
        }

        private Message CreateDeferredPointerMessage(Message message)
        {
            var deferredPointer = new Message
            {
                Body = Encoding.UTF8.GetBytes(message.SystemProperties.SequenceNumber.ToString()),
                MessageId = Guid.NewGuid().ToString(),
                PartitionKey = message.PartitionKey,
                ScheduledEnqueueTimeUtc = DateTime.UtcNow + BackOffDelayStrategy.Calculate(message.SystemProperties.DeliveryCount)
            };
            deferredPointer.UserProperties.Add(Constants.DeferredKey, message.SystemProperties.SequenceNumber);

            return deferredPointer;
        }
    }
}