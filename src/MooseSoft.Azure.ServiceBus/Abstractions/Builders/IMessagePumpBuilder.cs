using Microsoft.Azure.ServiceBus.Core;
using Moosesoft.Azure.ServiceBus.Builders;

namespace Moosesoft.Azure.ServiceBus.Abstractions.Builders
{
    /// <summary>
    /// Defines a builder for adding message pumps to <see cref="IMessageReceiver"/>.
    /// </summary>
    public interface IMessagePumpBuilder : IBuilder
    {
        /// <summary>
        /// Build a message pump within a <see cref="IMessageReceiver"/> for processing Azure Service Bus messages.
        /// </summary>
        /// <param name="options"></param>
        /// <returns>The <see cref="IMessageReceiver"/> the message pump built upon.</returns>
        IMessageReceiver Build(MessagePumpBuilderOptions options = null);
    }
}