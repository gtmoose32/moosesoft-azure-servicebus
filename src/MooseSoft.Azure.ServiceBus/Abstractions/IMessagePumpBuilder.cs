using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using System;
using System.Threading.Tasks;

namespace MooseSoft.Azure.ServiceBus.Abstractions
{
    public interface IMessagePumpBuilder
    {
        IMessageReceiver BuildMessagePump(
            Func<ExceptionReceivedEventArgs, Task> exceptionHandler,
            int maxConcurrentCalls = 10,
            Func<Exception, bool> shouldCompleteOnException = null);
    }
}