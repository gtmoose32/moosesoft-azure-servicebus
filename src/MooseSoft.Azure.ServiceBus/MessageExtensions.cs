using Microsoft.Azure.ServiceBus;
using System;
using System.Text;

namespace MooseSoft.Azure.ServiceBus
{
    internal static class MessageExtensions
    {
        private static bool HasDeferredKey(this Message message)
            => message.UserProperties.ContainsKey(Constants.DeferredKey) &&
               message.UserProperties[Constants.DeferredKey] != null;

        public static long? GetDeferredSequenceNumber(this Message message)
            => message.HasDeferredKey()
                ? (long) message.UserProperties[Constants.DeferredKey]
                : null as long?;

        public static int GetRetryCount(this Message message) => 
            message.UserProperties.ContainsKey(Constants.RetryCountKey) && message.UserProperties[Constants.RetryCountKey] != null
                ? (int) message.UserProperties[Constants.RetryCountKey]
                : 0;

        public static Message CreateDeferredLocatorMessage(this Message message, TimeSpan backOffDelay)
        {
            var deferredPointer = new Message
            {
                Body = Encoding.UTF8.GetBytes(message.SystemProperties.SequenceNumber.ToString()),
                MessageId = Guid.NewGuid().ToString(),
                PartitionKey = message.PartitionKey,
                ScheduledEnqueueTimeUtc = DateTime.UtcNow + backOffDelay
            };
            deferredPointer.UserProperties.Add(Constants.DeferredKey, message.SystemProperties.SequenceNumber);

            return deferredPointer;
        }
    }
}