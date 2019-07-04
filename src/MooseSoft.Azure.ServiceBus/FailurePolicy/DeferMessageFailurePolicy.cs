using MooseSoft.Azure.ServiceBus.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace MooseSoft.Azure.ServiceBus.FailurePolicy
{
    public class DeferMessageFailurePolicy : FailurePolicyBase
    {
        public DeferMessageFailurePolicy(Func<Exception, bool> canHandle, int maxDeliveryCount, IBackOffDelayStrategy backOffDelayStrategy = null) 
            : base(canHandle, maxDeliveryCount, backOffDelayStrategy)
        {
        } 

        public override async Task HandleFailureAsync(MessageContext context, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (context.Message.SystemProperties.DeliveryCount >= MaxDeliveryCount)
            {
                await context.MessageReceiver.AbandonAsync(context.Message.SystemProperties.LockToken).ConfigureAwait(false);
                return;
            }

            var deferredPointer = context.Message.CreateDeferredLocatorMessage(
                BackOffDelayStrategy.Calculate(context.Message.SystemProperties.DeliveryCount));

            var sender = context.CreateMessageSender();
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    await sender.SendAsync(deferredPointer).ConfigureAwait(false);
                    await context.MessageReceiver.DeferAsync(context.Message.SystemProperties.LockToken)
                        .ConfigureAwait(false);
                    scope.Complete();
                }
            }
            finally
            {
                await sender.CloseAsync();
            }
        }
    }
}