using Moosesoft.Azure.ServiceBus.Abstractions;
using System;

namespace Moosesoft.Azure.ServiceBus.BackOffDelayStrategy
{
    /// <summary>
    /// This strategy performs back off delay calculations using a linear model.
    /// </summary>
    public class LinearBackOffDelayStrategy : IBackOffDelayStrategy
    {
        private readonly TimeSpan _initialDelay;

        public LinearBackOffDelayStrategy(TimeSpan initialDelay)
        {
            _initialDelay = initialDelay;
        }

        /// <inheritdoc cref="IBackOffDelayStrategy"/>
        public virtual TimeSpan Calculate(int attempts) => TimeSpan.FromSeconds(_initialDelay.TotalSeconds * attempts);

        /// <summary>
        /// Creates an instance of this back off delay strategy with default settings.
        /// </summary>
        public static IBackOffDelayStrategy Default => new LinearBackOffDelayStrategy(TimeSpan.FromMinutes(1));
    }
}