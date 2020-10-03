using Microsoft.Azure.ServiceBus.Core;
using Moosesoft.Azure.ServiceBus.Builders;

namespace Moosesoft.Azure.ServiceBus.Abstractions.Builders
{
    public interface IMessagePumpBuilder : IBuilder
    {
        IMessageReceiver Build(MessagePumpBuilderOptions options = null);
    }
}