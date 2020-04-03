using Microsoft.Azure.ServiceBus;
using System;
using System.Threading.Tasks;

namespace MooseSoft.Azure.ServiceBus.Abstractions.MessagePumpBuilder
{
    public interface IMessagePumpBuilder
    {
        void BuildMessagePump(
            Func<ExceptionReceivedEventArgs, Task> exceptionHandler,
            int maxConcurrentCalls = 10,
            Func<Exception, bool> shouldCompleteOnException = null);
    }
}