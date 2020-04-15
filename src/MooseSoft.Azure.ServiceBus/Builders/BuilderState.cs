using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using MooseSoft.Azure.ServiceBus.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MooseSoft.Azure.ServiceBus.Builders
{
    public class BuilderState
    {
        public IMessageReceiver MessageReceiver { get; set; }
        public IBackOffDelayStrategy BackOffDelayStrategy { get; set; }
        public IFailurePolicy FailurePolicy { get; set; }
        public Type FailurePolicyType { get; set; }
        public Func<Exception, bool> CanHandle { get; set; }
        public IMessageProcessor MessageProcessor { get; set; }
        public Func<Message, CancellationToken, Task> ProcessMessageAsync { get; set; }
    }
}