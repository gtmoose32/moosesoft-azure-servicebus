using System;
using System.Threading;
using System.Threading.Tasks;

namespace MooseSoft.Azure.ServiceBus.Abstractions
{
    /// <summary>
    /// 
    /// </summary>
    public interface IFailurePolicy
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        bool CanHandle(Exception exception);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task HandleFailureAsync(MessageContext context, CancellationToken cancellationToken);
    }
}