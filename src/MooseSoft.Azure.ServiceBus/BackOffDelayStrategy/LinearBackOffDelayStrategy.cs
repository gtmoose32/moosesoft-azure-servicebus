using MooseSoft.Azure.ServiceBus.Abstractions;
using System;

namespace MooseSoft.Azure.ServiceBus.BackOffDelayStrategy
{
    public class LinearBackOffDelayStrategy : IBackOffDelayStrategy
    {
        private readonly int _multiplier;

        public LinearBackOffDelayStrategy(int multiplier)
        {
            _multiplier = multiplier;
        }

        public virtual TimeSpan Calculate(int attempts) => TimeSpan.FromMinutes(_multiplier * attempts);

        public static IBackOffDelayStrategy Default => new LinearBackOffDelayStrategy(2);
    }
}