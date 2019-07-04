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
        /// <summary>
        /// 
        /// </summary>
        public AbandonMessageFailurePolicy() : base(ex => false)
        {
        }

        /// <summary>
        /// This method will do nothing as it should never be called with this failure policy
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task HandleFailureAsync(MessageContext context, CancellationToken cancellationToken)
            => Task.CompletedTask;
    }
}