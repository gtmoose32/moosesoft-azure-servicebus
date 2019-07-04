using MooseSoft.Azure.ServiceBus.Abstractions;
using System;

namespace MooseSoft.Azure.ServiceBus.BackOffDelayStrategy
{
    /// <summary>
    /// 
    /// </summary>
    public class LinearBackOffDelayStrategy : IBackOffDelayStrategy
    {
        private readonly int _multiplier;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="multiplier"></param>
        public LinearBackOffDelayStrategy(int multiplier)
        {
            _multiplier = multiplier;
        }

        public virtual TimeSpan Calculate(int attempts) => TimeSpan.FromMinutes(_multiplier * attempts);

        /// <summary>
        /// 
        /// </summary>
        public static IBackOffDelayStrategy Default => new LinearBackOffDelayStrategy(2);
    }
}