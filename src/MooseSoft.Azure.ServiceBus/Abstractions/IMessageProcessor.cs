using Microsoft.Azure.ServiceBus;
using System.Threading;
using System.Threading.Tasks;

namespace Moosesoft.Azure.ServiceBus.Abstractions
{
    /// <summary>
    /// Defines an object that processes a Service Bus <see cref="Message"/>
    /// </summary>
    public interface IMessageProcessor
    {
        /// <summary>
        /// Processes a Service Bus <see cref="Message"/>
        /// </summary>
        /// <param name="message">Message received from a Service Bus entity for processing.</param>
        /// <param name="cancellationToken">Cancellation token used to check for cancellation upstream.</param>
        /// <returns><see cref="Task"/></returns>
        Task ProcessMessageAsync(Message message, CancellationToken cancellationToken);
    }
}