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

        /// <summary>
        /// Initialize a new instance <see cref="IFailurePolicy"/>.
        /// </summary>
        /// <param name="canHandle">Function that determines if the exception that has occurred <see cref="IFailurePolicy"/> will be applied to it.</param>
        /// <param name="backOffDelayStrategy"><see cref="IBackOffDelayStrategy"/> to use when <see cref="IFailurePolicy"/> is applied.</param>
        /// <param name="maxDeliveryCount">Maximum number of message delivery counts from Azure Service Bus.</param>
        protected FailurePolicyBase(Func<Exception, bool> canHandle, IBackOffDelayStrategy backOffDelayStrategy = null, int maxDeliveryCount = 10)
        {
            _canHandle = canHandle ?? throw new ArgumentNullException(nameof(canHandle));
            BackOffDelayStrategy = backOffDelayStrategy ?? new ZeroBackOffDelayStrategy();
            MaxDeliveryCount = maxDeliveryCount >= 0 ? maxDeliveryCount : 10;
        }

        /// <inheritdoc />
        public bool CanHandle(Exception exception) => _canHandle(exception);

        /// <inheritdoc />
        public abstract Task HandleFailureAsync(MessageContext context, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the delivery count for the specified Service Bus <see cref="Message"/>.
        /// </summary>
        /// <param name="message">Message used to determine the delivery count.</param>
        /// <returns>Number of times the message has been delivered.</returns>
        protected virtual int GetDeliveryCount(Message message) => message.GetDeliveryCount();
    }
}