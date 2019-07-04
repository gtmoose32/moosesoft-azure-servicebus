using MooseSoft.Azure.ServiceBus.Abstractions;
using System;
using System.Linq;

namespace MooseSoft.Azure.ServiceBus.BackOffDelayStrategy
{
    /// <summary>
    /// 
    /// </summary>
    public class ExponentialBackOffDelayStrategy : IBackOffDelayStrategy
    {
        private const int DefaultMaxRetryMinutes = 60;

        private readonly TimeSpan _maxRetryInterval;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxRetryMinutes"></param>
        public ExponentialBackOffDelayStrategy(int maxRetryMinutes)
        {
            _maxRetryInterval = TimeSpan.FromMinutes(maxRetryMinutes > 0 ? maxRetryMinutes : DefaultMaxRetryMinutes);
        }

        public virtual TimeSpan Calculate(int attempts)
            => new[] { TimeSpan.FromSeconds(100 * Math.Pow(attempts, 2)), _maxRetryInterval }.Min();

        /// <summary>
        /// 
        /// </summary>
        public static IBackOffDelayStrategy Default = new ExponentialBackOffDelayStrategy(DefaultMaxRetryMinutes);
    }
}