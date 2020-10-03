using Moosesoft.Azure.ServiceBus.Abstractions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Moosesoft.Azure.ServiceBus.Tests.Support
{
    [ExcludeFromCodeCoverage]
    public class TestFailurePolicy : IFailurePolicy
    {
        public bool CanHandle(Exception exception)
        {
            throw new NotImplementedException();
        }

        public Task HandleFailureAsync(MessageContext context, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}