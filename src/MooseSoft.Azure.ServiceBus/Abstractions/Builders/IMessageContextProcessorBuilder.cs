using System;

namespace MooseSoft.Azure.ServiceBus.Abstractions.Builders
{
    public interface IMessageContextProcessorBuilder : IBuilder
    {
        IMessageContextProcessor Build(Func<Exception, bool> shouldCompleteOn = null);
    }
}