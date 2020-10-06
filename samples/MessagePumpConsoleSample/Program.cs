using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Moosesoft.Azure.ServiceBus;
using Moosesoft.Azure.ServiceBus.Builders;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace MessagePumpConsoleSample
{
    [ExcludeFromCodeCoverage]
    internal class Program
    {
        static void Main()
        {
            //Replace connection string with a valid one
            var connection = new ServiceBusConnection("Endpoint=sb://somesbns.servicebus.windows.net/;SharedAccessKeyName=some-key;SharedAccessKey=fjsdjfkjsdakfjaskfjdskljfkdlsaf=");
            //Replace entity path with path to real entity on your ServiceBus namespace
            var receiver = new MessageReceiver(connection, "test");

            //Create message pump options
            var options = new MessagePumpBuilderOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 10,
                ShouldCompleteOnException = ex => ex is ArgumentOutOfRangeException
            };

            //Setup message pump with failure policy and back off delay strategy.
            receiver.ConfigureMessagePump()
                .WithMessageProcessor<SampleMessageProcessor>()
                .WithCloneMessageFailurePolicy(e => e is InvalidOperationException)
                .WithExponentialBackOffDelayStrategy()
                .Build(options);

            Console.WriteLine("Press any key to terminate!");
            Console.Read();
        }

        private static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs arg)
        {
            //Handle or log any exceptions that might bubble up from the message pump.
            return Task.CompletedTask;
        }
    }
}
