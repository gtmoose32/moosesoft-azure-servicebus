using MooseSoft.Azure.ServiceBus.BackOffDelayStrategy;
using MooseSoft.Azure.ServiceBus.Builders;
using MooseSoft.Azure.ServiceBus.FailurePolicy;
using System;

namespace MooseSoft.Azure.ServiceBus.Abstractions.Builders
{
    public abstract class BuilderBase<TBuilder> 
        : IMessageProcessorHolder<TBuilder>, IBackDelayStrategyHolder<TBuilder>, IFailurePolicyHolder<TBuilder>
        where TBuilder : IBuilder
    {
        protected BuilderState BuilderState { get; }

        protected internal BuilderBase()
        {
            BuilderState = new BuilderState();
        }

        #region IMessageProcessorHolder Members
        public IFailurePolicyHolder<TBuilder> WithMessageProcessor<TProcessor>(TProcessor messageProcessor)
            where TProcessor : IMessageProcessor
        {
            BuilderState.MessageProcessor = messageProcessor;
            return this;
        }

        public IFailurePolicyHolder<TBuilder> WithMessageProcessor<TProcessor>() where TProcessor : IMessageProcessor, new()
        {
            return WithMessageProcessor(new TProcessor());
        }
        #endregion

        #region IBackDelayStrategyHolder Members
        public abstract TBuilder WithBackOffDelayStrategy<TStrategy>(TStrategy backOffDelayStrategy)
            where TStrategy : IBackOffDelayStrategy;

        public TBuilder WithBackOffDelayStrategy<TStrategy>()
            where TStrategy : IBackOffDelayStrategy, new()
        {
            return WithBackOffDelayStrategy(new TStrategy());
        }

        public TBuilder WithExponentialBackOffDelayStrategy() =>
            WithBackOffDelayStrategy(ExponentialBackOffDelayStrategy.Default);

        public TBuilder WithExponentialBackOffDelayStrategy(TimeSpan maxDelay) =>
            WithBackOffDelayStrategy(new ExponentialBackOffDelayStrategy(maxDelay));

        public TBuilder WithConstantBackOffDelayStrategy() =>
            WithBackOffDelayStrategy(ConstantBackOffDelayStrategy.Default);

        public TBuilder WithConstantBackOffDelayStrategy(TimeSpan delayTime) =>
            WithBackOffDelayStrategy(new ConstantBackOffDelayStrategy(delayTime));

        public TBuilder WithLinearBackOffDelayStrategy() =>
            WithBackOffDelayStrategy(LinearBackOffDelayStrategy.Default);

        public TBuilder WithLinearBackOffDelayStrategy(TimeSpan delayTime) =>
            WithBackOffDelayStrategy(new LinearBackOffDelayStrategy(delayTime));

        public TBuilder WithZeroBackOffDelayStrategy() =>
            WithBackOffDelayStrategy(new ZeroBackOffDelayStrategy());
        #endregion

        #region IFailurePolicyHolder Members
        public IBackDelayStrategyHolder<TBuilder> WithCloneMessageFailurePolicy(Func<Exception, bool> canHandle = null)
        {
            SetFailurePolicyInfo(typeof(CloneMessageFailurePolicy), canHandle);
            return this;
        }

        public IBackDelayStrategyHolder<TBuilder> WithDeferMessageFailurePolicy(Func<Exception, bool> canHandle = null)
        {
            SetFailurePolicyInfo(typeof(DeferMessageFailurePolicy), canHandle);
            return this;
        }

        public abstract TBuilder WithAbandonMessageFailurePolicy();

        public abstract TBuilder WithFailurePolicy<TFailurePolicy>(TFailurePolicy failurePolicy)
            where TFailurePolicy : IFailurePolicy;
        #endregion

        protected IFailurePolicy CreateFailurePolicy()
        {
            return BuilderState.FailurePolicyType == typeof(CloneMessageFailurePolicy)
                ? new CloneMessageFailurePolicy(BuilderState.CanHandle, BuilderState.BackOffDelayStrategy)
                : (IFailurePolicy)new DeferMessageFailurePolicy(BuilderState.CanHandle, BuilderState.BackOffDelayStrategy);
        }

        internal static bool DefaultCanHandle(Exception exception) => true;

        internal BuilderState GetBuilderState() => BuilderState;

        private void SetFailurePolicyInfo(Type failurePolicyType, Func<Exception, bool> canHandle)
        {
            BuilderState.FailurePolicyType = failurePolicyType;
            BuilderState.CanHandle = canHandle ?? DefaultCanHandle;
        }
    }
}