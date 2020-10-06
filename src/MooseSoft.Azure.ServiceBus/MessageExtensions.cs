using Microsoft.Azure.ServiceBus;
using System;
using System.Text;

namespace Moosesoft.Azure.ServiceBus
{
    internal static class MessageExtensions
    {
        public static bool IsDeferredMessageLocator(this Message message)
            => message.Label != null &&
               message.Label.Equals(Constants.DeferredKey, StringComparison.OrdinalIgnoreCase);

        public static bool TryGetDeferredSequenceNumber(this Message message, out long sequenceNumber) 
            => long.TryParse(message.CorrelationId, out sequenceNumber);

        public static int GetRetryCount(this Message message) =>
            message.UserProperties.ContainsKey(Constants.RetryCountKey) && message.UserProperties[Constants.RetryCountKey] != null
                ? (int)message.UserProperties[Constants.RetryCountKey]
                : 0;

        public static Message CreateDeferredMessageLocator(this Message message, TimeSpan backOffDelay) =>
            new Message
            {
                Body = Encoding.UTF8.GetBytes(message.SystemProperties.SequenceNumber.ToString()),
                CorrelationId = message.SystemProperties.SequenceNumber.ToString(),
                MessageId = Guid.NewGuid().ToString(),
                Label = Constants.DeferredKey,
                PartitionKey = message.PartitionKey,
                ScheduledEnqueueTimeUtc = DateTime.UtcNow + backOffDelay
            };

        public static int GetDeliveryCount(this Message message) => message.SystemProperties.DeliveryCount;
    }
}