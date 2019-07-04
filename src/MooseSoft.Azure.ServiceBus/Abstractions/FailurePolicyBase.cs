using MooseSoft.Azure.ServiceBus.BackOffDelayStrategy;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MooseSoft.Azure.ServiceBus.Abstractions
{
    /// <inheritdoc cref="IFailurePolicy"/>>
    public abstract class FailurePolicyBase : IFailurePolicy
    {
        /// <summary>
        /// 
        /// </summary>
        protected int MaxDeliveryCount { get; }

        /// <summary>
        /// 
        /// </summary>
        protected IBackOffDelayStrategy BackOffDelayStrategy { get; }

        private readonly Func<Exception, bool> _canHandle;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="canHandle"></param>
        /// <param name="maxDeliveryCount"></param>
        /// <param name="backOffDelayStrategy"></param>
        protected FailurePolicyBase(Func<Exception, bool> canHandle, int maxDeliveryCount = 10, IBackOffDelayStrategy backOffDelayStrategy = null)
        {
            _canHandle = canHandle;
            MaxDeliveryCount = maxDeliveryCount;
            BackOffDelayStrategy = backOffDelayStrategy ?? new ZeroBackOffDelayStrategy();

        }

        public bool CanHandle(Exception exception) => _canHandle(exception);

        public abstract Task HandleFailureAsync(MessageContext context, CancellationToken cancellationToken);
    }
}