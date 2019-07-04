using System;

namespace MooseSoft.Azure.ServiceBus.Abstractions
{
    public interface IBackOffDelayStrategy
    {
        TimeSpan Calculate(int attempts);
    }
}