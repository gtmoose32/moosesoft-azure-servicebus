using Microsoft.Azure.ServiceBus;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;

namespace MooseSoft.Azure.ServiceBus.Tests
{
    [ExcludeFromCodeCoverage]
    public abstract class MessageTestBase
    {
        protected Message CreateMessage(int deliveryCount = 1)
        {
            var message = new Message
            {
                Body = Encoding.UTF8.GetBytes("Test!"),
                ContentType = "application/json",
                MessageId = Guid.NewGuid().ToString()
            };

            typeof(Message.SystemPropertiesCollection)
                .GetField("lockTokenGuid", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.SetValue(message.SystemProperties, Guid.NewGuid());

            typeof(Message.SystemPropertiesCollection)
                .GetField("sequenceNumber", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.SetValue(message.SystemProperties, 123);

            typeof(Message.SystemPropertiesCollection)
                .GetField("deliveryCount", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.SetValue(message.SystemProperties, deliveryCount);

            return message;
        }
    }
}