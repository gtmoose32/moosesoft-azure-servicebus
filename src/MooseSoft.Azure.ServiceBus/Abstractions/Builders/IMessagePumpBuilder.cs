using Microsoft.Azure.ServiceBus.Core;

namespace MooseSoft.Azure.ServiceBus.Abstractions.Builders
{
    public interface IMessagePumpBuilder : IBuilder
    {
        IMessageReceiver Build();
    }
}