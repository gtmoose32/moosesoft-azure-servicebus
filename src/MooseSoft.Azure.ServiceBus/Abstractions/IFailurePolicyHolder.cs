using System;

namespace MooseSoft.Azure.ServiceBus.Abstractions
{
    public interface IFailurePolicyHolder
    {
        IBackDelayStrategyHolder WithCloneFailurePolicy(Func<Exception, bool> canHandle = null);
        IBackDelayStrategyHolder WithDeferFailurePolicy(Func<Exception, bool> canHandle = null);
    }
}