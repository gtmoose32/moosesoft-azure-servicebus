namespace MooseSoft.Azure.ServiceBus.Abstractions
{
    public interface IMessageProcessorHolder
    {
        IFailurePolicyHolder WithMessageProcessor(IMessageProcessor messageProcessor);
    }
}