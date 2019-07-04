using Microsoft.Azure.ServiceBus;

namespace MooseSoft.Azure.ServiceBus
{
    public static class MessageExtensions
    {
        public const string DeferredKey = "DeferredSequenceNumber";
        public const string RetryCountKey = "RetryCount";

        private static bool HasDeferredKey(this Message message)
            => message.UserProperties.ContainsKey(DeferredKey) &&
               message.UserProperties[DeferredKey] != null;

        public static long? GetDeferredSequenceNumber(this Message message)
            => message.HasDeferredKey()
                ? (long) message.UserProperties[DeferredKey]
                : null as long?;

        public static int GetRetryCount(this Message message) => 
            message.UserProperties.ContainsKey(RetryCountKey) && message.UserProperties[RetryCountKey] != null
                ? (int) message.UserProperties[RetryCountKey]
                : 0;
    }
}