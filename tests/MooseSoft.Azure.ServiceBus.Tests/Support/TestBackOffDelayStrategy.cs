using Moosesoft.Azure.ServiceBus.Abstractions;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Moosesoft.Azure.ServiceBus.Tests.Support
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