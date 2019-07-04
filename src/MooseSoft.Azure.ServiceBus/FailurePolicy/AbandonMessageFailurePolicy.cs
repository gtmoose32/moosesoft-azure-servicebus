using MooseSoft.Azure.ServiceBus.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace MooseSoft.Azure.ServiceBus.FailurePolicy
{
    public class AbandonMessageFailurePolicy : FailurePolicyBase
    {
        public AbandonMessageFailurePolicy() : base(ex => false)
        {
        }

        //Do nothing since this will never be called
        public override Task HandleFailureAsync(MessageContext context, CancellationToken cancellationToken)
            => Task.CompletedTask;
    }
}