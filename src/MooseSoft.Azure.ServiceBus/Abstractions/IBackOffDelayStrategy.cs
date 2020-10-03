using System;

namespace Moosesoft.Azure.ServiceBus.Abstractions
{
    /// <summary>
    /// Defines a strategy for calculating back off delay for message processing retries.
    /// </summary>
    public interface IBackOffDelayStrategy
    {
        /// <summary>
        /// Performs calculation to determine how much time to wait for next retry.
        /// </summary>
        /// <param name="attempts">The number of attempts to process the message.</param>
        /// <returns>Delay that should be applied to the message before it is retried.</returns>
        TimeSpan Calculate(int attempts);
    }
}