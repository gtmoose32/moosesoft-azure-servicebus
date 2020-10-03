using Microsoft.Azure.ServiceBus;
using Moosesoft.Azure.ServiceBus.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Moosesoft.Azure.ServiceBus.FailurePolicy
{
    /// <summary>
    /// This failure policy will create a clone of the Service Bus Message attempting to be processed.
    /// The clone will then be sent back to Service Bus while the original message is completed in an atomic transaction with the send.
    /// New messages sent to Service Bus will be delayed based upon the <see cref="IBackOffDelayStrategy"/> chosen.
    /// </summary>
    public class CloneMessageFailurePolicy : FailurePolicyBase
    {
        public CloneMessageFailurePolicy(
            Func<Exception, bool> canHandle, 
            IBackOffDelayStrategy backOffDelayStrategy = null, 
            int maxDeliveryCount = 10)
            : base(canHandle, backOffDelayStrategy, maxDeliveryCount)
        {
        } 

        public override async Task HandleFailureAsync(MessageContext context, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var deliveryCount = GetDeliveryCount(context.Message);
            if (deliveryCount >= MaxDeliveryCount)
            {
                await context.MessageReceiver.DeadLetterAsync(
                        context.Message.SystemProperties.LockToken,
                        $"Max delivery count of {MaxDeliveryCount} has been reached.")
                    .ConfigureAwait(false);

                return;
            }

            var clone = context.Message.Clone();
            clone.MessageId = Guid.NewGuid().ToString();
            clone.ScheduledEnqueueTimeUtc = DateTime.UtcNow + BackOffDelayStrategy.Calculate(deliveryCount);
            clone.UserProperties[Constants.RetryCountKey] = deliveryCount;

            var sender = context.CreateMessageSender();
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    await sender.SendAsync(clone).ConfigureAwait(false);
                    await context.MessageReceiver.CompleteAsync(context.Message.SystemProperties.LockToken)
                        .ConfigureAwait(false);

                    scope.Complete();
                }

            }
            finally
            {
                await sender.CloseAsync();
            }
        }

        protected override int GetDeliveryCount(Message message)
        {
            return base.GetDeliveryCount(message) + message.GetRetryCount(); 
        }
    }
}