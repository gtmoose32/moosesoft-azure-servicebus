using MooseSoft.Azure.ServiceBus.Abstractions;
using System;

namespace MooseSoft.Azure.ServiceBus.BackOffDelayStrategy
{
    /// <summary>
    /// This strategy provides a constant delay for every Calculate call regardless of attempt count.
    /// </summary>
    public class ConstantBackOffDelayStrategy : IBackOffDelayStrategy
    {
        private static readonly TimeSpan DefaultBackOffDelayTime = TimeSpan.FromMinutes(5);

        private readonly TimeSpan _backOffDelay;

        public ConstantBackOffDelayStrategy(TimeSpan backOffDelayTime)
        {
            _backOffDelay = backOffDelayTime >= TimeSpan.Zero ? backOffDelayTime : DefaultBackOffDelayTime;
        }

        /// <inheritdoc cref="IBackOffDelayStrategy"/>
        public TimeSpan Calculate(int attempts) => _backOffDelay;

        /// <summary>
        /// Creates an instance of this back off delay strategy with default settings.
        /// </summary>
        public static IBackOffDelayStrategy Default => new ConstantBackOffDelayStrategy(DefaultBackOffDelayTime);
    }
}