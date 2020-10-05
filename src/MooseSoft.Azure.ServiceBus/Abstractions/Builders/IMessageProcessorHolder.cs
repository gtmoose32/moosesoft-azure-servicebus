using Microsoft.Azure.ServiceBus;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Moosesoft.Azure.ServiceBus.Abstractions.Builders
{
    /// <summary>
    /// Interface that defines a set of methods for specifying different implementations of <see cref="IMessageProcessor"/>.
    /// </summary>
    /// <typeparam name="TBuilder">Type of builder.</typeparam>
    public interface IMessageProcessorHolder<out TBuilder>
        where TBuilder : IBuilder
    {
        /// <summary>
        /// Set an instance of <see cref="IMessageProcessor"/> to be used for configuration within a <see cref="IBuilder"/>.
        /// </summary>
        /// <param name="messageProcessor"><see cref="IMessageProcessor"/> instance to be used by the <see cref="IBuilder"/>.</param>
        /// <typeparam name="TProcessor">Type of <see cref="IMessageProcessor"/></typeparam>
        /// <returns><see cref="IFailurePolicyHolder{TBuilder}"/></returns>
        IFailurePolicyHolder<TBuilder> WithMessageProcessor<TProcessor>(TProcessor messageProcessor)
            where TProcessor : IMessageProcessor;

        /// <summary>
        /// Set an instance of <see cref="IMessageProcessor"/> to be used for configuration within a <see cref="IBuilder"/>.
        /// </summary>
        /// <typeparam name="TProcessor">Type of <see cref="IMessageProcessor"/></typeparam>
        /// <returns><see cref="IFailurePolicyHolder{TBuilder}"/></returns>
        IFailurePolicyHolder<TBuilder> WithMessageProcessor<TProcessor>()
            where TProcessor : IMessageProcessor, new();

        /// <summary>
        /// Set a function pointer to be used for message processing to be used for configuration within a <see cref="IBuilder"/> instead of an instance of <see cref="IMessageProcessor"/>.
        /// </summary>
        /// <returns><see cref="IFailurePolicyHolder{TBuilder}"/></returns>
        IFailurePolicyHolder<TBuilder> WithMessageProcessor(Func<Message, CancellationToken, Task> processMessage);
    }
}