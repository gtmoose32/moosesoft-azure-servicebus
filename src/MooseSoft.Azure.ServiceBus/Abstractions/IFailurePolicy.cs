using System;
using System.Threading;
using System.Threading.Tasks;

namespace Moosesoft.Azure.ServiceBus.Abstractions
{
    /// <summary>
    /// Defines a failure policy which describes how to handle message processing failures.
    /// </summary>
    public interface IFailurePolicy
    {
        /// <summary>
        /// Determines whether the type of exception that has occurred should be handled
        /// </summary>
        /// <param name="exception">Exception that has occurred while processing a message.</param>
        /// <returns>A <see cref="bool"/> which indicates whether to handle this exception or not.</returns>
        bool CanHandle(Exception exception);

        /// <summary>
        /// Handles Service Bus Message processing failures
        /// </summary>
        /// <param name="context">The context the Service Bus Message resides in.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns><see cref="Task"/></returns>
        Task HandleFailureAsync(MessageContext context, CancellationToken cancellationToken);
    }
}