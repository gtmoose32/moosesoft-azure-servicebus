namespace MooseSoft.Azure.ServiceBus.Abstractions
{
    public interface IMessageProcessorHolder
    {
        IFailurePolicyHolder WithMessageProcessor<T>(T messageProcessor)
            where T : IMessageProcessor;

        IFailurePolicyHolder WithMessageProcessor<T>()
            where T : IMessageProcessor, new();
    }
}