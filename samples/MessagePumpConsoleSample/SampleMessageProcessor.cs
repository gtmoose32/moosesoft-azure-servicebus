using Microsoft.Azure.ServiceBus;
using MooseSoft.Azure.ServiceBus.Abstractions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace MessagePumpConsoleSample
{
    /// <summary>
    /// Implementations of <see cref="IMessageProcessor"/> should process messages received from Service Bus.
    /// This sample class is merely to demonstrate failure policies with back off delays.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SampleMessageProcessor : IMessageProcessor
    {
        public Task ProcessMessageAsync(Message message, CancellationToken cancellationToken)
        {
            throw new InvalidOperationException("Test failure policy with back off delay strategy.");
        }
    }
}