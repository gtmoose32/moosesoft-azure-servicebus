using MooseSoft.Azure.ServiceBus.Abstractions;
using System;
using System.Linq;

namespace MooseSoft.Azure.ServiceBus.BackOffDelayStrategy
{
    /// <summary>
    /// This strategy performs back off delay calculations using a exponential model.
    /// </summary>
    public class ExponentialBackOffDelayStrategy : IBackOffDelayStrategy
    {
        private const int DefaultMaxDelayMinutes = 60;

        private readonly TimeSpan _maxDelay;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxDelayMinutes"></param>
        public ExponentialBackOffDelayStrategy(int maxDelayMinutes)
        {
            _maxDelay = TimeSpan.FromMinutes(maxDelayMinutes > 0 ? maxDelayMinutes : DefaultMaxDelayMinutes);
        }

        /// <inheritdoc cref="IBackOffDelayStrategy"/>
        public virtual TimeSpan Calculate(int attempts)
            => new[] { TimeSpan.FromSeconds(100 * Math.Pow(attempts, 2)), _maxDelay }.Min();

        /// <summary>
        /// Creates an instance of this back off delay strategy with default settings.
        /// </summary>
        public static IBackOffDelayStrategy Default = new ExponentialBackOffDelayStrategy(DefaultMaxDelayMinutes);
    }
}