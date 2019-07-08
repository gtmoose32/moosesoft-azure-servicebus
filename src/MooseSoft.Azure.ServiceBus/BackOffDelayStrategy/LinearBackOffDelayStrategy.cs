using MooseSoft.Azure.ServiceBus.Abstractions;
using System;

namespace MooseSoft.Azure.ServiceBus.BackOffDelayStrategy
{
    /// <summary>
    /// This strategy performs back off delay calculations using a linear model.
    /// </summary>
    public class LinearBackOffDelayStrategy : IBackOffDelayStrategy
    {
        private readonly int _multiplier;

        public LinearBackOffDelayStrategy(int multiplier)
        {
            _multiplier = multiplier;
        }

        /// <inheritdoc cref="IBackOffDelayStrategy"/>
        public virtual TimeSpan Calculate(int attempts) => TimeSpan.FromMinutes(_multiplier * attempts);

        /// <summary>
        /// Creates an instance of this back off delay strategy with default settings.
        /// </summary>
        public static IBackOffDelayStrategy Default => new LinearBackOffDelayStrategy(2);
    }
}