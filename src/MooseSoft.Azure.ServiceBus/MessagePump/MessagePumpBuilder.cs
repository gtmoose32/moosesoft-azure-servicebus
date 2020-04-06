using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using MooseSoft.Azure.ServiceBus.Abstractions;
using MooseSoft.Azure.ServiceBus.BackOffDelayStrategy;
using MooseSoft.Azure.ServiceBus.FailurePolicy;
using System;
using System.Threading.Tasks;

namespace MooseSoft.Azure.ServiceBus.MessagePump
{
    internal class MessagePumpBuilder 
        : IFailurePolicyHolder, IBackDelayStrategyHolder, IMessagePumpBuilder, IMessageProcessorHolder
    {
        private readonly MessagePumpBuilderState _builderState;

        public MessagePumpBuilder(IMessageReceiver messageReceiver)
        {
            _builderState = new MessagePumpBuilderState
            {
                MessageReceiver = messageReceiver
            };
        }

        #region IFailurePolicyHolder Members
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

        public IBackDelayStrategyHolder WithAbandonFailurePolicy()
        {
            return WithFailurePolicy(new AbandonMessageFailurePolicy());
        }

        public IBackDelayStrategyHolder WithFailurePolicy<T>(T failurePolicy) where T : IFailurePolicy
        {
            _builderState.FailurePolicy = failurePolicy;
            return this;
        } 
        #endregion

        #region IBackOffDelayStrategyHolder Members
        public IMessagePumpBuilder WithBackOffDelayStrategy<T>(T backOffDelayStrategy) where T : IBackOffDelayStrategy
        {
            _builderState.BackOffDelayStrategy = backOffDelayStrategy;
            return this;
        }

        public IMessagePumpBuilder WithBackOffDelayStrategy<T>() where T : IBackOffDelayStrategy, new()
        {
            return WithBackOffDelayStrategy(new T());
        }

        public IMessagePumpBuilder WithExponentialBackOffDelayStrategy() =>
            WithBackOffDelayStrategy(ExponentialBackOffDelayStrategy.Default);

        public IMessagePumpBuilder WithExponentialBackOffDelayStrategy(TimeSpan maxDelay) =>
            WithBackOffDelayStrategy(new ExponentialBackOffDelayStrategy(maxDelay));

        public IMessagePumpBuilder WithConstantBackOffDelayStrategy() =>
            WithBackOffDelayStrategy(ConstantBackOffDelayStrategy.Default);

        public IMessagePumpBuilder WithConstantBackOffDelayStrategy(TimeSpan delayTime) =>
            WithBackOffDelayStrategy(new ConstantBackOffDelayStrategy(delayTime));

        public IMessagePumpBuilder WithLinearBackOffDelayStrategy() =>
            WithBackOffDelayStrategy(LinearBackOffDelayStrategy.Default);

        public IMessagePumpBuilder WithLinearBackOffDelayStrategy(TimeSpan delayTime) =>
            WithBackOffDelayStrategy(new LinearBackOffDelayStrategy(delayTime));

        public IMessagePumpBuilder WithZeroBackOffDelayStrategy() =>
            WithBackOffDelayStrategy(new ZeroBackOffDelayStrategy());
        #endregion

        #region IMessagePumpBuilder Members
        public IMessageReceiver BuildMessagePump(
            Func<ExceptionReceivedEventArgs, Task> exceptionHandler,
            int maxConcurrentCalls = 10,
            Func<Exception, bool> shouldCompleteOnException = null)
        {
            var contextProcessor = new MessageContextProcessor(
                _builderState.MessageProcessor,
                _builderState.FailurePolicy ?? CreateFailurePolicy(),
                shouldCompleteOnException);

            var options = new MessageHandlerOptions(exceptionHandler)
            {
                AutoComplete = false,
                MaxConcurrentCalls = maxConcurrentCalls
            };

            _builderState.MessageReceiver.RegisterMessageHandler(
                (message, token) => contextProcessor.ProcessMessageContextAsync(
                    new MessageContext(message, _builderState.MessageReceiver), token),
                options);

            return _builderState.MessageReceiver;
        } 
        #endregion

        #region IMessageProcessorHolder Members
        public IFailurePolicyHolder WithMessageProcessor<T>(T messageProcessor)
            where T : IMessageProcessor
        {
            _builderState.MessageProcessor = messageProcessor;
            return this;
        }

        public IFailurePolicyHolder WithMessageProcessor<T>() where T : IMessageProcessor, new()
        {
            return WithMessageProcessor(new T());
        }
        #endregion

        private IFailurePolicy CreateFailurePolicy()
        {
            return _builderState.FailurePolicyType == typeof(CloneMessageFailurePolicy)
                ? new CloneMessageFailurePolicy(_builderState.CanHandle, _builderState.BackOffDelayStrategy)
                : (IFailurePolicy)new DeferMessageFailurePolicy(_builderState.CanHandle, _builderState.BackOffDelayStrategy);
        }

        private static bool DefaultCanHandle(Exception exception) => true;

        private void SetFailurePolicyInfo(Type failurePolicyType, Func<Exception, bool> canHandle)
        {
            _builderState.FailurePolicyType = failurePolicyType;
            _builderState.CanHandle = canHandle ?? DefaultCanHandle;
        }
    }
}