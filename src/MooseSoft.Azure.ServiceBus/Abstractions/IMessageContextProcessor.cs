using System.Threading;
using System.Threading.Tasks;

namespace MooseSoft.Azure.ServiceBus.Abstractions
{
    public interface IMessageContextProcessor
    {
        Task ProcessMessageContextAsync(MessageContext context, CancellationToken cancellationToken = default);
    }
}