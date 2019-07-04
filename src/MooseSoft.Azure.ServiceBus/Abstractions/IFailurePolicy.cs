using System;
using System.Threading;
using System.Threading.Tasks;

namespace MooseSoft.Azure.ServiceBus.Abstractions
{
    public interface IFailurePolicy
    {
        bool CanHandle(Exception exception);

        Task HandleFailureAsync(MessageContext context, CancellationToken cancellationToken);
    }
}