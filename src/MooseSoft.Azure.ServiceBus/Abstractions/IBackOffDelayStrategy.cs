using System;

namespace MooseSoft.Azure.ServiceBus.Abstractions
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBackOffDelayStrategy
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="attempts"></param>
        /// <returns></returns>
        TimeSpan Calculate(int attempts);
    }
}