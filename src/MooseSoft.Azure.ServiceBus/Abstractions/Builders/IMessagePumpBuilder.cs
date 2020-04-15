using Microsoft.Azure.ServiceBus.Core;
using MooseSoft.Azure.ServiceBus.Builders;

namespace MooseSoft.Azure.ServiceBus.Abstractions.Builders
{
    public interface IMessagePumpBuilder : IBuilder
    {
        IMessageReceiver Build(MessagePumpBuilderOptions options = null);
    }
}