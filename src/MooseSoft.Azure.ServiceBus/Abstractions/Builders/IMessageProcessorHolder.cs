using Microsoft.Azure.ServiceBus;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Moosesoft.Azure.ServiceBus.Abstractions.Builders
{
    public interface IMessageProcessorHolder<out TBuilder>
        where TBuilder : IBuilder
    {
        IFailurePolicyHolder<TBuilder> WithMessageProcessor<TProcessor>(TProcessor messageProcessor)
            where TProcessor : IMessageProcessor;

        IFailurePolicyHolder<TBuilder> WithMessageProcessor<TProcessor>()
            where TProcessor : IMessageProcessor, new();

        IFailurePolicyHolder<TBuilder> WithMessageProcessor(Func<Message, CancellationToken, Task> processMessage);
    }
}