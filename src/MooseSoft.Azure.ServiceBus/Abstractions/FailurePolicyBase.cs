using Microsoft.Azure.ServiceBus;
using Moosesoft.Azure.ServiceBus.BackOffDelayStrategy;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Moosesoft.Azure.ServiceBus.Abstractions
{
    /// <inheritdoc cref="IFailurePolicy"/>>
    public abstract class FailurePolicyBase : IFailurePolicy
    {
        /// <summary>
        /// Maximum delivery count allowed
        /// </summary>
        protected int MaxDeliveryCount { get; }

        /// <summary>
        /// Back off delay calculator strategy
        /// </summary>
        protected IBackOffDelayStrategy BackOffDelayStrategy { get; }

        private readonly Func<Exception, bool> _canHandle;

        protected FailurePolicyBase(Func<Exception, bool> canHandle, IBackOffDelayStrategy backOffDelayStrategy = null, int maxDeliveryCount = 10)
        {
            _canHandle = canHandle ?? throw new ArgumentNullException(nameof(canHandle));
            BackOffDelayStrategy = backOffDelayStrategy ?? new ZeroBackOffDelayStrategy();
            MaxDeliveryCount = maxDeliveryCount >= 0 ? maxDeliveryCount : 10;
        }

        public bool CanHandle(Exception exception) => _canHandle(exception);

        public abstract Task HandleFailureAsync(MessageContext context, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the delivery count for the specified Service Bus <see cref="Message"/>.
        /// </summary>
        /// <param name="message">Message used to determine the delivery count.</param>
        /// <returns>Number of times the message has been delivered.</returns>
        protected virtual int GetDeliveryCount(Message message) => message.GetDeliveryCount();
    }
}