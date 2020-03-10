using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using MooseSoft.Azure.ServiceBus;
using MooseSoft.Azure.ServiceBus.BackOffDelayStrategy;
using MooseSoft.Azure.ServiceBus.FailurePolicy;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace MessagePumpConsoleSample
{
    [ExcludeFromCodeCoverage]
    class Program
    {
        static void Main()
        {
            var connection = new ServiceBusConnection("Endpoint=sb://somesbns.servicebus.windows.net/;SharedAccessKeyName=some-key;SharedAccessKey=fjsdjfkjsdakfjaskfjdskljfkdlsaf=");
            
            var contextProcessor = new MessageContextProcessor(
                new SampleMessageProcessor(),
                new DeferMessageFailurePolicy(e => true, new ZeroBackOffDelayStrategy()));

            var receiver = new MessageReceiver(connection, "test");
            //Add deferred message plugin
            receiver.AddDeferredMessagePlugin();
            //Setup message pump to use message context processor with defer failure policy.
            receiver.RegisterMessageHandler(
                (message, token) => contextProcessor.ProcessMessageContextAsync(new MessageContext(message, receiver), token), 
                ExceptionReceivedHandler);

    		Console.WriteLine("Press any key to terminate!");
            Console.Read();
        }

        private static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs arg)
        {
            //Handle any exception that might bubble up from the message pump however this shouldn't really happen 
            return Task.CompletedTask;
        }
    }
}
