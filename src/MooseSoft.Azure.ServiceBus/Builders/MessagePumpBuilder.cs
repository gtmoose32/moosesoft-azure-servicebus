using Microsoft.Azure.ServiceBus.Core;
using MooseSoft.Azure.ServiceBus.Abstractions.Builders;
using System;

namespace MooseSoft.Azure.ServiceBus.Builders
{
    public class MessagePumpBuilder : BuilderBase<IMessagePumpBuilder>, IMessagePumpBuilder
    {
        public MessagePumpBuilder(IMessageReceiver receiver)
        {
            BuilderState.MessageReceiver = receiver;
        }

        public override IMessagePumpBuilder WithBackOffDelayStrategy<TStrategy>(TStrategy backOffDelayStrategy)
        {
            throw new NotImplementedException();
        }

        public override IMessagePumpBuilder WithAbandonMessageFailurePolicy()
        {
            throw new NotImplementedException();
        }

        public override IMessagePumpBuilder WithFailurePolicy<TFailurePolicy>(TFailurePolicy failurePolicy)
        {
            throw new NotImplementedException();
        }

        public IMessageReceiver Build()
        {
            throw new NotImplementedException();
        }
    }
}