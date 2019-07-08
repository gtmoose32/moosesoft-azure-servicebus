using System.Threading;
using System.Threading.Tasks;

namespace MooseSoft.Azure.ServiceBus.Abstractions
{
    /// <summary>
    /// Defines an object that processes a <see cref="MessageContext"./>
    /// </summary>
    public interface IMessageContextProcessor
    {
        /// <summary>
        /// Processes the received <see cref="MessageContext"/>
        /// </summary>
        /// <param name="context">Context to be processed.</param>
        /// <param name="cancellationToken">Optional cancellation token provided to check for cancellation upstream.</param>
        /// <returns><see cref="Task"/></returns>
        Task ProcessMessageContextAsync(MessageContext context, CancellationToken cancellationToken = default);
    }
}