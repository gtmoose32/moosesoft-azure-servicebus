using MooseSoft.Azure.ServiceBus.Abstractions;
using System;
using System.Linq;

namespace MooseSoft.Azure.ServiceBus.BackOffDelayStrategy
{
    public class ExponentialBackOffDelayStrategy : IBackOffDelayStrategy
    {
        private const int DefaultMaxRetryMinutes = 60;

        private readonly TimeSpan _maxRetryInterval;

        public ExponentialBackOffDelayStrategy(int maxRetryMinutes)
        {
            _maxRetryInterval = TimeSpan.FromMinutes(maxRetryMinutes > 0 ? maxRetryMinutes : DefaultMaxRetryMinutes);
        }

        public virtual TimeSpan Calculate(int attempts)
            => new[] { TimeSpan.FromSeconds(100 * Math.Pow(attempts, 2)), _maxRetryInterval }.Min();

        public static IBackOffDelayStrategy Default = new ExponentialBackOffDelayStrategy(DefaultMaxRetryMinutes);
    }
}