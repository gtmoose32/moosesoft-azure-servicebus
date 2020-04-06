using System;

namespace MooseSoft.Azure.ServiceBus.Abstractions
{
    public interface IBackDelayStrategyHolder
    {
        IMessagePumpBuilder WithBackOffDelayStrategy<T>(T backOffDelayStrategy)
            where T : IBackOffDelayStrategy;

        IMessagePumpBuilder WithBackOffDelayStrategy<T>()
            where T : IBackOffDelayStrategy, new();

        IMessagePumpBuilder WithExponentialBackOffDelayStrategy();

        IMessagePumpBuilder WithExponentialBackOffDelayStrategy(TimeSpan maxDelay);

        IMessagePumpBuilder WithConstantBackOffDelayStrategy();
        IMessagePumpBuilder WithConstantBackOffDelayStrategy(TimeSpan delayTime);

        IMessagePumpBuilder WithLinearBackOffDelayStrategy();
        IMessagePumpBuilder WithLinearBackOffDelayStrategy(TimeSpan delayTime);
        IMessagePumpBuilder WithZeroBackOffDelayStrategy();

    }
}