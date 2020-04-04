using Microsoft.Azure.ServiceBus;
using MooseSoft.Azure.ServiceBus.Abstractions;
using PipelineFramework.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MooseSoft.Azure.ServiceBus.PipelineFramework
{
    public abstract class AsyncPipelineMessageProcessorBase<TPayload> : IMessageProcessor
        where TPayload : class
    {
        private readonly IAsyncPipeline<TPayload> _pipeline;
        private readonly IPayloadVerifier<TPayload> _payloadVerifier;

        protected AsyncPipelineMessageProcessorBase(IAsyncPipeline<TPayload> pipeline, IPayloadVerifier<TPayload> payloadVerifier = null)
        {
            _pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));
            _payloadVerifier = payloadVerifier;
        }

        public virtual async Task ProcessMessageAsync(Message message, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var payload = await _pipeline.ExecuteAsync(CreatePipelinePayload(message), cancellationToken).ConfigureAwait(false);

            if (_payloadVerifier == null) return;

            await _payloadVerifier.VerifyAsync(payload, cancellationToken).ConfigureAwait(false);
        }

        protected abstract TPayload CreatePipelinePayload(Message message);
    }
}
