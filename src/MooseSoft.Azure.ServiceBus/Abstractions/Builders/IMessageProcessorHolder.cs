using Microsoft.Azure.ServiceBus;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MooseSoft.Azure.ServiceBus.Abstractions.Builders
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMessageProcessorHolder<out TBuilder>
        where TBuilder : IBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TBuilder"></typeparam>
        /// <typeparam name="TProcessor"></typeparam>
        /// <param name="messageProcessor"></param>
        /// <returns></returns>
        IFailurePolicyHolder<TBuilder> WithMessageProcessor<TProcessor>(TProcessor messageProcessor)
            where TProcessor : IMessageProcessor;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TBuilder"></typeparam>
        /// <typeparam name="TProcessor"></typeparam>
        /// <returns></returns>
        IFailurePolicyHolder<TBuilder> WithMessageProcessor<TProcessor>()
            where TProcessor : IMessageProcessor, new();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="processMessage"></param>
        /// <returns></returns>
        IFailurePolicyHolder<TBuilder> WithMessageProcessor(Func<Message, CancellationToken, Task> processMessage);
    }
}