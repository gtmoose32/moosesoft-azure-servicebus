using Microsoft.Azure.ServiceBus.Core;
using Moosesoft.Azure.ServiceBus.Abstractions.Builders;
using Moosesoft.Azure.ServiceBus.Builders;

namespace Moosesoft.Azure.ServiceBus
{
    public static class Builder
    {
        public static class MessageContextProcessor
        {
            public static IMessageProcessorHolder<IMessageContextProcessorBuilder> Configure()
            {
                return new MessageContextProcessorBuilder();
            }
        }

        public static class MessagePump
        {
            public static IMessageProcessorHolder<IMessagePumpBuilder> Configure(IMessageReceiver messageReceiver)
            {
                return new MessagePumpBuilder(messageReceiver);
            }
        }
    }
}