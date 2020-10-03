using Microsoft.Azure.ServiceBus;
using MooseSoft.Azure.ServiceBus.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MooseSoft.Azure.ServiceBus
{
    internal class DefaultMessageProcessor : IMessageProcessor
    {
        private readonly Func<Message, CancellationToken, Task> _processMessage;

        public DefaultMessageProcessor(Func<Message, CancellationToken, Task> processMessage)
        {
            _processMessage = processMessage;
        }

        public Task ProcessMessageAsync(Message message, CancellationToken cancellationToken) =>
            _processMessage(message, cancellationToken);

    }
}