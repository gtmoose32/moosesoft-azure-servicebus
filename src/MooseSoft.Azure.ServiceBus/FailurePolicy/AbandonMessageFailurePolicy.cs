using MooseSoft.Azure.ServiceBus.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace MooseSoft.Azure.ServiceBus.FailurePolicy
{
    /// <summary>
    /// This failure policy will abandon the message for any exceptions that occur.
    /// </summary>
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