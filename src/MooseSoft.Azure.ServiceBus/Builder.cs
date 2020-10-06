using Microsoft.Azure.ServiceBus.Core;
using Moosesoft.Azure.ServiceBus.Abstractions.Builders;
using Moosesoft.Azure.ServiceBus.Builders;

namespace Moosesoft.Azure.ServiceBus
{
    /// <summary>
    /// Builder used to create MessageContextProcessors and MessagePumps for Azure Service Bus
    /// </summary>
    public static class Builder
    {
        /// <summary>
        /// Builder class used to build MessageContextProcessor instances for applying Azure Service Bus failure policies.
        /// </summary>
        public static IMessageProcessorHolder<IMessageContextProcessorBuilder> ConfigureMessageContextProcessor()
            => new MessageContextProcessorBuilder();

        /// <summary>
        /// Builder class used to build MessagePumps for processing messages while applying Azure Service Bus failure policies.
        /// </summary>
        public static IMessageProcessorHolder<IMessagePumpBuilder> ConfigureMessagePump(IMessageReceiver messageReceiver)
            => new MessagePumpBuilder(messageReceiver);
    }
}