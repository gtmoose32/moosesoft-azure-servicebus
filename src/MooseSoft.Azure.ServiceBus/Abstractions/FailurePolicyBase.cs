using MooseSoft.Azure.ServiceBus.FailurePolicy;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MooseSoft.Azure.ServiceBus.Abstractions
{
    public abstract class FailurePolicyBase : IFailurePolicy
    {
        protected FailurePolicySettings Settings { get; }
        private readonly Func<Exception, bool> _canHandle;

        protected FailurePolicyBase(Func<Exception, bool> canHandle)
            : this(canHandle, FailurePolicySettings.Default)
        {
        }

        protected FailurePolicyBase(Func<Exception, bool> canHandle, FailurePolicySettings settings)
        {
            Settings = settings;
            _canHandle = canHandle;
        }

        public bool CanHandle(Exception exception) => _canHandle(exception);

        public abstract Task HandleFailureAsync(MessageContext context, CancellationToken cancellationToken);

        protected virtual TimeSpan CalculateBackOffDelay(int attempts)
            => new[] { TimeSpan.FromSeconds(100 * Math.Pow(attempts, 2)), Settings.MaxRetryInterval }.Min();
    }
}