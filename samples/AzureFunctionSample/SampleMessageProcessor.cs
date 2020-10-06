using Microsoft.Azure.ServiceBus;
using Moosesoft.Azure.ServiceBus.Abstractions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace AzureFunctionSample
{
    /// <summary>
    /// Implementations of <see cref="IMessageProcessor"/> should process messages received from Service Bus.
    /// This sample class is merely to demonstrate failure policies with back off delays.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SampleMessageProcessor : IMessageProcessor
    {
        private readonly Random _random;

        public SampleMessageProcessor()
        {
            _random = new Random();
        }

        public Task ProcessMessageAsync(Message message, CancellationToken cancellationToken)
        {
            var num = _random.Next(0, 10);
            if (num == 0) // throw an out of range exception to demonstrate should complete on.
                throw new ArgumentOutOfRangeException(nameof(num));

            //throw exception to demonstrate failure policy on even numbers
            if (num % 2 != 0)
                throw new InvalidOperationException("Test failure policy with back off delay strategy.");

            return Task.CompletedTask; //complete on odd non-zero numbers
        }
    }
}