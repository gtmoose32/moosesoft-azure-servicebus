using Microsoft.Azure.ServiceBus;
using MooseSoft.Azure.ServiceBus.Abstractions;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace AzureFunctionSample
{
    /// <summary>
    /// This class should contain code custom code necessary to "process" the message received from Service Bus
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SampleMessageProcessor : IMessageProcessor
    {
        public Task ProcessMessageAsync(Message message, CancellationToken cancellationToken)
        {
            //TODO: Add real processing logic
            return Task.CompletedTask;
        }
    }
}