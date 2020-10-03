using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Moosesoft.Azure.ServiceBus;
using Moosesoft.Azure.ServiceBus.Abstractions;
using Moosesoft.Azure.ServiceBus.BackOffDelayStrategy;
using Moosesoft.Azure.ServiceBus.FailurePolicy;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace AzureFunctionSample
{
    [ExcludeFromCodeCoverage]
    public static class SampleFunction
    {
        private static readonly IMessageContextProcessor MessageContextProcessor;

        /// <summary>
        /// This will create a new MessageContextProcessor with DeferMessageFailurePolicy and default ExponentialBackOffDelayStrategy.
        /// This is a simple demon.  In a more real world scenario, you might want to consider injecting <see cref="IMessageContextProcessor"/> instead
        /// for increased testability.  
        /// </summary>
        static SampleFunction()
        {
            MessageContextProcessor = new MessageContextProcessor(
                new SampleMessageProcessor(),
                new CloneMessageFailurePolicy(ex => true, ExponentialBackOffDelayStrategy.Default));
        }

        [FunctionName("SampleFunction")]
        public static async Task RunAsync(
            [ServiceBusTrigger("%ServiceBusEntityName%", Connection = "ServiceBusConnectionString")]
            Message message, 
            MessageReceiver messageReceiver, 
            ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {message.MessageId}");

            await MessageContextProcessor.ProcessMessageContextAsync(new MessageContext(message, messageReceiver))
                .ConfigureAwait(false);
        }
    }
}
