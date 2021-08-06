using Moosesoft.Azure.ServiceBus.Abstractions;
using Moosesoft.Azure.ServiceBus.Abstractions.Builders;
using Moosesoft.Azure.ServiceBus.FailurePolicy;
using System;

namespace Moosesoft.Azure.ServiceBus.Builders
{
    internal class MessageContextProcessorBuilder : BuilderBase<IMessageContextProcessorBuilder>, IMessageContextProcessorBuilder
    {
        public override IMessageContextProcessorBuilder WithBackOffDelayStrategy<TStrategy>(TStrategy backOffDelayStrategy)
        {
            BuilderState.BackOffDelayStrategy = backOffDelayStrategy;
            return this;
        }

        public override IMessageContextProcessorBuilder WithAbandonMessageFailurePolicy()
        {
            BuilderState.FailurePolicy = new AbandonMessageFailurePolicy();
            return this;
        }

        public override IMessageContextProcessorBuilder WithFailurePolicy<TFailurePolicy>(TFailurePolicy failurePolicy)
        {
            BuilderState.FailurePolicy = failurePolicy;
            return this;
        }

        public IMessageContextProcessor Build(Func<Exception, bool> shouldCompleteOn = null) =>
            CreateMessageContextProcessor(shouldCompleteOn);

        public IMessageContextProcessor Build(string name, Func<Exception, bool> shouldCompleteOn = null) =>
            CreateMessageContextProcessor(name, shouldCompleteOn);
    }
}