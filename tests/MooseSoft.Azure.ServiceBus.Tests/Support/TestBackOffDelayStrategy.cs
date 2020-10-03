using MooseSoft.Azure.ServiceBus.Abstractions;
using System;
using System.Diagnostics.CodeAnalysis;

namespace MooseSoft.Azure.ServiceBus.Tests.Support
{
    [ExcludeFromCodeCoverage]
    public class TestBackOffDelayStrategy : IBackOffDelayStrategy
    {
        public TimeSpan Calculate(int attempts)
        {
            return TimeSpan.Zero;
        }
    }
}