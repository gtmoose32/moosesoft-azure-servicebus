using Moosesoft.Azure.ServiceBus.Abstractions;
using System;
using System.Linq;

namespace Moosesoft.Azure.ServiceBus.BackOffDelayStrategy
{
    /// <summary>
    /// This strategy performs back off delay calculations using a exponential model.
    /// </summary>
    public class ExponentialBackOffDelayStrategy : IBackOffDelayStrategy
    {
        private static readonly TimeSpan DefaultMaxDelayTime = TimeSpan.FromMinutes(60);

        private static readonly TimeSpan DefaultInitialDelay = TimeSpan.FromSeconds(100);

        private readonly TimeSpan _maxDelay;

        private readonly int _initialBackOffDelaySeconds;

        /// <summary>
        /// Initialize a new instance <see cref="ExponentialBackOffDelayStrategy"/>.
        /// </summary>
        /// <param name="maxDelayTime"></param>
        public ExponentialBackOffDelayStrategy(TimeSpan maxDelayTime) : this(maxDelayTime, DefaultInitialDelay)
        {
            
        }

        /// <summary>
        /// Initialize a new instance <see cref="ExponentialBackOffDelayStrategy"/>.
        /// </summary>
        /// <param name="maxDelayTime">The maximum back off that would be calculated</param>
        /// <param name="initialDelay">The initial back off</param>
        public ExponentialBackOffDelayStrategy(TimeSpan maxDelayTime, TimeSpan initialDelay)
        {
            _maxDelay = maxDelayTime > TimeSpan.Zero ? maxDelayTime : DefaultMaxDelayTime;
            _initialBackOffDelaySeconds = (int) (initialDelay > TimeSpan.Zero ? initialDelay : DefaultInitialDelay).TotalSeconds;
        }

        /// <inheritdoc cref="IBackOffDelayStrategy"/>
        public virtual TimeSpan Calculate(int attempts)
            => new[] { TimeSpan.FromSeconds(_initialBackOffDelaySeconds * Math.Pow(attempts, 2)), _maxDelay }.Min();

        /// <summary>
        /// Creates an instance of this back off delay strategy with default settings.
        /// </summary>
        public static readonly IBackOffDelayStrategy Default = new ExponentialBackOffDelayStrategy(DefaultMaxDelayTime);
    }
}