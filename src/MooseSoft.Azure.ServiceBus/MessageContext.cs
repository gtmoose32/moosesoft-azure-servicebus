using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using System;

namespace MooseSoft.Azure.ServiceBus
{
    public class MessageContext
    {
        public Message Message { get; set; }
        public IMessageReceiver MessageReceiver { get; }

        public IMessageSender CreateMessageSender() => new MessageSender(MessageReceiver.ServiceBusConnection, MessageReceiver.Path);

        public MessageContext(Message message, IMessageReceiver messageReceiver)
        {
            Message = message ?? throw new ArgumentNullException(nameof(message));
            MessageReceiver = messageReceiver ?? throw new ArgumentNullException(nameof(messageReceiver));
        }
    }
}
