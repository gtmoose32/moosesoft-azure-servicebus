using System;

namespace MooseSoft.Azure.ServiceBus.FailurePolicy
{
    public class FailurePolicySettings
    {
        private const int DefaultMaxRetryMinutes = 60;
        private const int DefaultMaxDeliveryCount = 10;

        public int MaxDeliveryCount { get; }
        public TimeSpan MaxRetryInterval { get; }

        #region ctor
        private FailurePolicySettings() : this(DefaultMaxDeliveryCount, DefaultMaxRetryMinutes)
        {

        }

        public FailurePolicySettings(int maxDeliveryCount, int maxRetryMinutes)
        {
            MaxDeliveryCount = maxDeliveryCount > 0 ? maxDeliveryCount : DefaultMaxDeliveryCount;
            MaxRetryInterval = TimeSpan.FromMinutes(maxRetryMinutes > 0 ? maxRetryMinutes : DefaultMaxRetryMinutes);
        } 
        #endregion

        public static FailurePolicySettings Default => new FailurePolicySettings();
    }
}