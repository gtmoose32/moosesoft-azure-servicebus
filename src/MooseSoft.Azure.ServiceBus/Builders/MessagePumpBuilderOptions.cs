using Microsoft.Azure.ServiceBus;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace MooseSoft.Azure.ServiceBus.Builders
{
    [ExcludeFromCodeCoverage]
    public class MessagePumpBuilderOptions
    {
        public MessagePumpBuilderOptions(Func<ExceptionReceivedEventArgs, Task> exceptionReceivedHandler)
        {
            ExceptionReceivedHandler = exceptionReceivedHandler
                ?? throw new ArgumentNullException(nameof(exceptionReceivedHandler));
        }

        public Func<ExceptionReceivedEventArgs, Task> ExceptionReceivedHandler { get; }

        public int MaxConcurrentCalls { get; set; } = 1;

        public TimeSpan MaxAutoRenewDuration { get; set; } = TimeSpan.FromMinutes(5);

        public Func<Exception, bool> ShouldCompleteOnException { get; set; }

        public static implicit operator MessageHandlerOptions(MessagePumpBuilderOptions options) =>
            options == null
                ? new MessageHandlerOptions(args => Task.CompletedTask) { AutoComplete = false }
                : new MessageHandlerOptions(options.ExceptionReceivedHandler)
                {
                    AutoComplete = false,
                    MaxConcurrentCalls = options.MaxConcurrentCalls,
                    MaxAutoRenewDuration = options.MaxAutoRenewDuration
                };
    }
}