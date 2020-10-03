using Microsoft.Azure.ServiceBus.Core;
using Moosesoft.Azure.ServiceBus.Abstractions;
using Moosesoft.Azure.ServiceBus.Abstractions.Builders;
using Moosesoft.Azure.ServiceBus.FailurePolicy;
using System;

namespace Moosesoft.Azure.ServiceBus.Builders
{
    internal class MessagePumpBuilder : BuilderBase<IMessagePumpBuilder>, IMessagePumpBuilder
    {
        public MessagePumpBuilder(IMessageReceiver receiver)
        {
            BuilderState.MessageReceiver = receiver;
        }

        public override IMessagePumpBuilder WithBackOffDelayStrategy<TStrategy>(TStrategy backOffDelayStrategy)
        {
            BuilderState.BackOffDelayStrategy = backOffDelayStrategy;
            return this;
        }

        public override IMessagePumpBuilder WithAbandonMessageFailurePolicy()
        {
            BuilderState.FailurePolicy = new AbandonMessageFailurePolicy();
            return this;
        }

        public override IMessagePumpBuilder WithFailurePolicy<TFailurePolicy>(TFailurePolicy failurePolicy)
        {
            BuilderState.FailurePolicy = failurePolicy;
            return this;
        }

        public IMessageReceiver Build(MessagePumpBuilderOptions options = null)
        {
            var contextProcessor = CreateMessageContextProcessor(options?.ShouldCompleteOnException);

            BuilderState.MessageReceiver.RegisterMessageHandler((message, token) => 
                    contextProcessor.ProcessMessageContextAsync(new MessageContext(message, BuilderState.MessageReceiver), token),
                options);

            return BuilderState.MessageReceiver;
        }

        private IMessageContextProcessor CreateMessageContextProcessor(Func<Exception, bool> shouldCompleteOnException = null) =>
            new MessageContextProcessor(
                BuilderState.MessageProcessor,
                BuilderState.FailurePolicy ?? CreateFailurePolicy(),
                shouldCompleteOnException);
    }
}