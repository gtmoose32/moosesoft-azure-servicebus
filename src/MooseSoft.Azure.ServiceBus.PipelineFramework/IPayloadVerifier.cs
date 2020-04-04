using System.Threading;
using System.Threading.Tasks;

namespace MooseSoft.Azure.ServiceBus.PipelineFramework
{
    public interface IPayloadVerifier<in TPayload>
        where TPayload : class
    {
        Task VerifyAsync(TPayload payload, CancellationToken cancellationToken);
    }
}