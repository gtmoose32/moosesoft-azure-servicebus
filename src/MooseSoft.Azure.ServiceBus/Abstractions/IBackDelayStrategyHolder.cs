namespace MooseSoft.Azure.ServiceBus.Abstractions
{
    public interface IBackDelayStrategyHolder
    {
        IMessagePumpBuilder WithBackOffDelayStrategy<T>(T backOffDelayStrategy)
            where T : IBackOffDelayStrategy;
    }
}