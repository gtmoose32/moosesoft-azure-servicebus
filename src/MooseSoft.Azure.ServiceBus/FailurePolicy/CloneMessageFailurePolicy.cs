using MooseSoft.Azure.ServiceBus.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace MooseSoft.Azure.ServiceBus.FailurePolicy
{
    public class CloneMessageFailurePolicy : FailurePolicyBase
    {
        #region ctor
        public CloneMessageFailurePolicy(Func<Exception, bool> canHandle, int maxDeliveryCount, IBackOffDelayStrategy backOffDelayStrategy = null)
            : base(canHandle, maxDeliveryCount, backOffDelayStrategy)
        {
        } 
        #endregion

        public override async Task HandleFailureAsync(MessageContext context, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var deliveryCount = context.Message.GetRetryCount() + context.Message.SystemProperties.DeliveryCount;
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

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await sender.SendAsync(clone).ConfigureAwait(false);
                await context.MessageReceiver.CompleteAsync(context.Message.SystemProperties.LockToken)
                    .ConfigureAwait(false);

                scope.Complete();
            }

            await sender.CloseAsync();
        }
    }
}