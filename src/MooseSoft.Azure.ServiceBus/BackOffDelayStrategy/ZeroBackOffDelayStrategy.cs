using Moosesoft.Azure.ServiceBus.Abstractions;
using System;

namespace Moosesoft.Azure.ServiceBus.BackOffDelayStrategy
{
    /// <summary>
    /// This strategy will always return zero back off delay for any call to Calculate.
    /// </summary>
    public class ZeroBackOffDelayStrategy : IBackOffDelayStrategy
    {
        /// <inheritdoc cref="IBackOffDelayStrategy"/>
        public TimeSpan Calculate(int attempts) => TimeSpan.Zero;
    }
}