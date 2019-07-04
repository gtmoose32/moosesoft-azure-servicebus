using MooseSoft.Azure.ServiceBus.BackOffDelayStrategy;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MooseSoft.Azure.ServiceBus.Abstractions
{
    public abstract class FailurePolicyBase : IFailurePolicy
    {
        protected int MaxDeliveryCount { get; }
        protected IBackOffDelayStrategy BackOffDelayStrategy { get; }

        private readonly Func<Exception, bool> _canHandle;

        protected FailurePolicyBase(Func<Exception, bool> canHandle, int maxDeliveryCount = 10, IBackOffDelayStrategy backOffDelayStrategy = null)
        {
            MaxDeliveryCount = maxDeliveryCount;
            BackOffDelayStrategy = backOffDelayStrategy ?? ExponentialBackOffDelayStrategy.Default;
            _canHandle = canHandle;
        }

        public bool CanHandle(Exception exception) => _canHandle(exception);

        public abstract Task HandleFailureAsync(MessageContext context, CancellationToken cancellationToken);
    }
}