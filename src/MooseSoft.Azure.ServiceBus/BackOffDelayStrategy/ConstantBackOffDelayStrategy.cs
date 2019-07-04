using MooseSoft.Azure.ServiceBus.Abstractions;
using System;

namespace MooseSoft.Azure.ServiceBus.BackOffDelayStrategy
{
    /// <summary>
    /// 
    /// </summary>
    public class ConstantBackOffDelayStrategy : IBackOffDelayStrategy
    {
        private const int DefaultBackOffDelayMinutes = 5;

        private readonly TimeSpan _backOffDelay;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="backOffDelayMinutes"></param>
        public ConstantBackOffDelayStrategy(int backOffDelayMinutes)
        {
            _backOffDelay = TimeSpan.FromMinutes(backOffDelayMinutes > 0 ? backOffDelayMinutes : DefaultBackOffDelayMinutes);    
        }

        public TimeSpan Calculate(int attempts) => _backOffDelay;

        /// <summary>
        /// 
        /// </summary>
        public static IBackOffDelayStrategy Default => new ConstantBackOffDelayStrategy(DefaultBackOffDelayMinutes);
    }
}