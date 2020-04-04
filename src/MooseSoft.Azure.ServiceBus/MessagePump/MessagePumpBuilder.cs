using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using MooseSoft.Azure.ServiceBus.Abstractions;
using MooseSoft.Azure.ServiceBus.FailurePolicy;
using System;
using System.Threading.Tasks;

namespace MooseSoft.Azure.ServiceBus.MessagePump
{
    internal class MessagePumpBuilder : IFailurePolicyHolder, IBackDelayStrategyHolder, IMessagePumpBuilder, IMessageProcessorHolder
    {
        private readonly MessagePumpBuilderState _builderState;

        public MessagePumpBuilder(IMessageReceiver messageReceiver)
        {
            _builderState = new MessagePumpBuilderState
            {
                MessageReceiver = messageReceiver
            };
        }

        private IFailurePolicy CreateFailurePolicy()
        {
            return _builderState.FailurePolicyType == typeof(CloneMessageFailurePolicy)
                ? new CloneMessageFailurePolicy(_builderState.CanHandle, _builderState.BackOffDelayStrategy)
                : (IFailurePolicy) new DeferMessageFailurePolicy(_builderState.CanHandle, _builderState.BackOffDelayStrategy);
        }

        public IBackDelayStrategyHolder WithCloneFailurePolicy(Func<Exception, bool> canHandle = null)
        {
            SetFailurePolicyInfo(typeof(CloneMessageFailurePolicy), canHandle);
            return this;
        }

        public IBackDelayStrategyHolder WithDeferFailurePolicy(Func<Exception, bool> canHandle = null)
        {
            SetFailurePolicyInfo(typeof(DeferMessageFailurePolicy), canHandle);
            return this;
        }

        public IMessagePumpBuilder WithBackOffDelayStrategy<T>(T backOffDelayStrategy) where T : IBackOffDelayStrategy
        {
            _builderState.BackOffDelayStrategy = backOffDelayStrategy;
            return this;
        }

        public IMessageReceiver BuildMessagePump(
            Func<ExceptionReceivedEventArgs, Task> exceptionHandler, 
            int maxConcurrentCalls = 10, 
            Func<Exception, bool> shouldCompleteOnException = null)
        {
            var contextProcessor = new MessageContextProcessor(_builderState.MessageProcessor, CreateFailurePolicy(), shouldCompleteOnException);
            var options = new MessageHandlerOptions(exceptionHandler)
            {
                AutoComplete = false, MaxConcurrentCalls = maxConcurrentCalls
            };

            _builderState.MessageReceiver.RegisterMessageHandler(
                (message, token) => contextProcessor.ProcessMessageContextAsync(
                    new MessageContext(message, _builderState.MessageReceiver), token), 
                options);

            return _builderState.MessageReceiver;
        }

        public IFailurePolicyHolder WithMessageProcessor(IMessageProcessor messageProcessor)
        {
            _builderState.MessageProcessor = messageProcessor;
            return this;
        }

        private static bool DefaultCanHandle(Exception exception) => true;

        private void SetFailurePolicyInfo(Type failurePolicyType, Func<Exception, bool> canHandle)
        {
            _builderState.FailurePolicyType = failurePolicyType;
            _builderState.CanHandle = canHandle ?? DefaultCanHandle;
        }
    }
}