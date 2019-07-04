using Microsoft.Azure.ServiceBus;
using System.Threading;
using System.Threading.Tasks;

namespace MooseSoft.Azure.ServiceBus.Abstractions
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMessageProcessor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task ProcessMessageAsync(Message message, CancellationToken cancellationToken);
    }
}