using Microsoft.Azure.ServiceBus;
using System.Threading;
using System.Threading.Tasks;

namespace MooseSoft.Azure.ServiceBus.Abstractions
{
    public interface IMessageProcessor
    {
        Task ProcessMessageAsync(Message message, CancellationToken cancellationToken = default);
    }
}