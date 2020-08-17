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
        private static readonly TimeSpan DefaultMaxDelayTime = TimeSpan.FromMinutes(60);

        private static readonly TimeSpan DefaultInitialDelay = TimeSpan.FromSeconds(100);

        private readonly TimeSpan _maxDelay;

        private readonly int _initialBackoffDelaySeconds;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxDelayTime"></param>
        public ExponentialBackOffDelayStrategy(TimeSpan maxDelayTime) : this(maxDelayTime, DefaultInitialDelay)
        {
            
        }

        /// <summary>
        /// Creates a new <see cref="ExponentialBackOffDelayStrategy"/> with a specified maxDelayTime and intialDelay
        /// </summary>
        /// <param name="maxDelayTime">The maximum backoff that would be calculated</param>
        /// <param name="initialDelay">The initial backoff</param>
        public ExponentialBackOffDelayStrategy(TimeSpan maxDelayTime, TimeSpan initialDelay)
        {
            _maxDelay = maxDelayTime > TimeSpan.Zero ? maxDelayTime : DefaultMaxDelayTime;
            _initialBackoffDelaySeconds = (int) (initialDelay > TimeSpan.Zero ? initialDelay : DefaultInitialDelay).TotalSeconds;
        }

        /// <inheritdoc cref="IBackOffDelayStrategy"/>
        public virtual TimeSpan Calculate(int attempts)
            => new[] { TimeSpan.FromSeconds(_initialBackoffDelaySeconds * Math.Pow(attempts, 2)), _maxDelay }.Min();

        /// <summary>
        /// Creates an instance of this back off delay strategy with default settings.
        /// </summary>
        public static readonly IBackOffDelayStrategy Default = new ExponentialBackOffDelayStrategy(DefaultMaxDelayTime);
    }
}