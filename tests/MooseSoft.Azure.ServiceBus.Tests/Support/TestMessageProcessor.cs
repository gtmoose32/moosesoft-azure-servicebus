using Microsoft.Azure.ServiceBus;
using Moosesoft.Azure.ServiceBus.Abstractions;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Moosesoft.Azure.ServiceBus.Tests.Support
{
    [ExcludeFromCodeCoverage]
    public class TestMessageProcessor : IMessageProcessor
    {
        public Task ProcessMessageAsync(Message message, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}