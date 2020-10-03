using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using System.Diagnostics.CodeAnalysis;

namespace Moosesoft.Azure.ServiceBus.Tests
{
    [ExcludeFromCodeCoverage]
    public class TestMessageContext : MessageContext
    {
        private readonly IMessageSender _messageSender;

        public TestMessageContext(Message message, IMessageReceiver messageReceiver, IMessageSender messageSender) 
            : base(message, messageReceiver)
        {
            _messageSender = messageSender;
        }

        public override IMessageSender CreateMessageSender() => _messageSender;
    }
}