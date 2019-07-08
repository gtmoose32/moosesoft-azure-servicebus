using System.Threading;
using System.Threading.Tasks;

namespace MooseSoft.Azure.ServiceBus.Abstractions
{
    /// <summary>
    /// Defines a 
    /// </summary>
    public interface IMessageContextProcessor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task ProcessMessageContextAsync(MessageContext context, CancellationToken cancellationToken = default);
    }
}