using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Moosesoft.Azure.ServiceBus;
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
            //Replace connection string with a valid one
            var connection = new ServiceBusConnection("Endpoint=sb://somesbns.servicebus.windows.net/;SharedAccessKeyName=some-key;SharedAccessKey=fjsdjfkjsdakfjaskfjdskljfkdlsaf=");
            //Replace entity path with path to real entity on your ServiceBus namespace
            var receiver = new MessageReceiver(connection, "test");

            //Setup message pump with failure policy and back off delay strategy.
            receiver.ConfigureMessagePump()
                .WithMessageProcessor<SampleMessageProcessor>()
                .WithCloneMessageFailurePolicy(e => e is InvalidOperationException)
                .WithExponentialBackOffDelayStrategy()
                .Build();

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
