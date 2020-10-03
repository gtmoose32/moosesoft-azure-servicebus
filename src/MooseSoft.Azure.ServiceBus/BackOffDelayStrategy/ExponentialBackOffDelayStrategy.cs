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

        private readonly TimeSpan _maxDelay;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxDelayTime"></param>
        public ExponentialBackOffDelayStrategy(TimeSpan maxDelayTime)
        {
            _maxDelay = maxDelayTime > TimeSpan.Zero ? maxDelayTime : DefaultMaxDelayTime;
        }

        /// <inheritdoc cref="IBackOffDelayStrategy"/>
        public virtual TimeSpan Calculate(int attempts)
            => new[] { TimeSpan.FromSeconds(100 * Math.Pow(attempts, 2)), _maxDelay }.Min();

        /// <summary>
        /// Creates an instance of this back off delay strategy with default settings.
        /// </summary>
        public static IBackOffDelayStrategy Default = new ExponentialBackOffDelayStrategy(DefaultMaxDelayTime);
    }
}