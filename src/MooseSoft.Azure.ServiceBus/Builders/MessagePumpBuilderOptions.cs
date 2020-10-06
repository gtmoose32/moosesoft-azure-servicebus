using Microsoft.Azure.ServiceBus;
using System;
using System.Threading.Tasks;

namespace Moosesoft.Azure.ServiceBus.Builders
{
    /// <summary>
    /// Provides a set of options for building message pumps for processing Azure Service Bus messages.
    /// </summary>
    public class MessagePumpBuilderOptions
    {
        /// <summary>Initializes a new instance of the <see cref="MessagePumpBuilderOptions" /> class.
        /// Default Values:
        ///     <see cref="MaxConcurrentCalls"/> = 1
        ///     <see cref="MaxAutoRenewDuration"/> = 5 minutes
        /// </summary>
        /// <param name="exceptionReceivedHandler">A <see cref="Func{T1, TResult}"/> that is invoked during exceptions. <see cref="ExceptionReceivedEventArgs"/> contains contextual information regarding the exception.</param>
        public MessagePumpBuilderOptions(Func<ExceptionReceivedEventArgs, Task> exceptionReceivedHandler)
        {
            ExceptionReceivedHandler = exceptionReceivedHandler
                ?? throw new ArgumentNullException(nameof(exceptionReceivedHandler));
        }

        /// <summary>
        /// Occurs when an exception is received. Enables you to be notified of any errors encountered by the message pump.  When errors are received calls will automatically be retried, so this is informational.
        /// </summary>
        public Func<ExceptionReceivedEventArgs, Task> ExceptionReceivedHandler { get; }

        /// <summary>
        /// Gets or sets the maximum number of concurrent calls to the callback the message pump should initiate.
        /// </summary>
        public int MaxConcurrentCalls { get; set; } = 1;

        /// <summary>
        /// Gets or sets the maximum duration within which the lock will be renewed automatically. This value should be greater than the longest message lock duration; for example, the LockDuration Property.
        /// </summary>
        public TimeSpan MaxAutoRenewDuration { get; set; } = TimeSpan.FromMinutes(5);

        /// <summary>
        /// Function that determines whether the message should be completed on certain exception(s).
        /// </summary>
        public Func<Exception, bool> ShouldCompleteOnException { get; set; }

        /// <summary>
        /// Implicit cast operator for converting <see cref="MessagePumpBuilderOptions"/> into <see cref="MessageHandlerOptions"/> implicitly.
        /// </summary>
        /// <param name="options">Options used for creating message pumps.</param>
        /// <returns><see cref="MessageHandlerOptions"/></returns>
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