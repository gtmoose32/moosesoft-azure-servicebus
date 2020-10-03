using Moosesoft.Azure.ServiceBus.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace Moosesoft.Azure.ServiceBus.FailurePolicy
{
    /// <summary>
    /// This failure policy will abandon the message for any exceptions that occur without any back off delay.
    /// </summary>
    public class AbandonMessageFailurePolicy : FailurePolicyBase
    {
        /// <summary>
        /// Creates a new instance.
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