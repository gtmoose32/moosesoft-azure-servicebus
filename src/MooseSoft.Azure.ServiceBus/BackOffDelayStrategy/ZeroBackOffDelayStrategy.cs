using MooseSoft.Azure.ServiceBus.Abstractions;
using System;

namespace MooseSoft.Azure.ServiceBus.BackOffDelayStrategy
{
    /// <summary>
    /// ZeroBackOffDelayStrategy will always return zero delay.
    /// </summary>
    public class ZeroBackOffDelayStrategy : IBackOffDelayStrategy
    {
        /// <inheritdoc cref="IBackOffDelayStrategy"/>
        public TimeSpan Calculate(int attempts) => TimeSpan.Zero;
    }
}