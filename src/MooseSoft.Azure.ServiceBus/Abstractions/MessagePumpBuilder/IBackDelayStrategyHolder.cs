namespace MooseSoft.Azure.ServiceBus.Abstractions.MessagePumpBuilder
{
    public interface IBackDelayStrategyHolder
    {
        IMessagePumpBuilder WithBackOffDelayStrategy<T>(T backOffDelayStrategy)
            where T : IBackOffDelayStrategy;
    }
}