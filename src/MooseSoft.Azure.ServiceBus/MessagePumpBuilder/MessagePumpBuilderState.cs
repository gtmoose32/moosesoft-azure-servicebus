using Microsoft.Azure.ServiceBus.Core;
using MooseSoft.Azure.ServiceBus.Abstractions;
using System;

namespace MooseSoft.Azure.ServiceBus.MessagePumpBuilder
{
    internal class MessagePumpBuilderState
    {
        public IMessageReceiver MessageReceiver { get; set; }
        public IBackOffDelayStrategy BackOffDelayStrategy { get; set; }
        public Type FailurePolicyType { get; set; }
        public Func<Exception, bool> CanHandle { get; set; }
        public IMessageProcessor MessageProcessor { get; set; }
    }
}