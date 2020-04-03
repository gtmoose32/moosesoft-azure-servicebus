namespace MooseSoft.Azure.ServiceBus.Abstractions.MessagePumpBuilder
{
    public interface IMessageProcessorHolder
    {
        IFailurePolicyHolder WithMessageProcessor(IMessageProcessor messageProcessor);
    }
}