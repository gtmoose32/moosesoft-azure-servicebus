using Microsoft.Azure.ServiceBus.Core;
using MooseSoft.Azure.ServiceBus.Abstractions.Builders;

namespace MooseSoft.Azure.ServiceBus.Builders
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