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
        /// Initialize a new instance <see cref="AbandonMessageFailurePolicy"/>.
        /// </summary>
        public AbandonMessageFailurePolicy() : base(ex => false)
        {
        }
        
        /// <inheritdoc />
        public override Task HandleFailureAsync(MessageContext context, CancellationToken cancellationToken)
            => Task.CompletedTask;
    }
}