using Microsoft.Azure.ServiceBus;

namespace MooseSoft.Azure.ServiceBus
{
    internal static class MessageExtensions
    {
        private static bool HasDeferredKey(this Message message)
            => message.UserProperties.ContainsKey(Constants.DeferredKey) &&
               message.UserProperties[Constants.DeferredKey] != null;

        internal static long? GetDeferredSequenceNumber(this Message message)
            => message.HasDeferredKey()
                ? (long) message.UserProperties[Constants.DeferredKey]
                : null as long?;

        internal static int GetRetryCount(this Message message) => 
            message.UserProperties.ContainsKey(Constants.RetryCountKey) && message.UserProperties[Constants.RetryCountKey] != null
                ? (int) message.UserProperties[Constants.RetryCountKey]
                : 0;
    }
}