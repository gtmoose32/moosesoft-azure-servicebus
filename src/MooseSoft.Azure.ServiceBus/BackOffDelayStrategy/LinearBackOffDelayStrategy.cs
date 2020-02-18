using MooseSoft.Azure.ServiceBus.Abstractions;
using System;

namespace MooseSoft.Azure.ServiceBus.BackOffDelayStrategy
{
    /// <summary>
    /// This strategy performs back off delay calculations using a linear model.
    /// </summary>
    public class LinearBackOffDelayStrategy : IBackOffDelayStrategy
    {
        private readonly TimeSpan _multiplier;

        public LinearBackOffDelayStrategy(TimeSpan multiplier)
        {
            _multiplier = multiplier;
        }

        /// <inheritdoc cref="IBackOffDelayStrategy"/>
        public virtual TimeSpan Calculate(int attempts) => TimeSpan.FromTicks(_multiplier.Ticks * attempts);

        /// <summary>
        /// Creates an instance of this back off delay strategy with default settings.
        /// </summary>
        public static IBackOffDelayStrategy Default => new LinearBackOffDelayStrategy(TimeSpan.FromMinutes(2));
    }
}