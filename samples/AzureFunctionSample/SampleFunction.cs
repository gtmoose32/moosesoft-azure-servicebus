using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Moosesoft.Azure.ServiceBus;
using Moosesoft.Azure.ServiceBus.Abstractions;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace AzureFunctionSample
{
    [ExcludeFromCodeCoverage]
    public class SampleFunction
    {
        private readonly IMessageContextProcessor _messageContextProcessor;
        private readonly ILogger _logger;

        public SampleFunction(IMessageContextProcessor messageContextProcessor, ILogger logger)
        {
            _messageContextProcessor = messageContextProcessor;
            _logger = logger;
        }

        [FunctionName("SampleFunction")]
        public async Task ProcessMessageAsync(
            [ServiceBusTrigger("%ServiceBusEntityName%", Connection = "ServiceBusConnectionString")]
            Message message, 
            MessageReceiver messageReceiver)
        {
            _logger.LogInformation($"C# ServiceBus queue trigger function processed message: {message.MessageId}");

            await _messageContextProcessor.ProcessMessageContextAsync(new MessageContext(message, messageReceiver))
                .ConfigureAwait(false);
        }
    }
}
