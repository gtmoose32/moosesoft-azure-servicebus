namespace MooseSoft.Azure.ServiceBus.Abstractions
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMessageProcessorHolder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messageProcessor"></param>
        /// <returns></returns>
        IFailurePolicyHolder WithMessageProcessor<T>(T messageProcessor)
            where T : IMessageProcessor;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IFailurePolicyHolder WithMessageProcessor<T>()
            where T : IMessageProcessor, new();
    }
}