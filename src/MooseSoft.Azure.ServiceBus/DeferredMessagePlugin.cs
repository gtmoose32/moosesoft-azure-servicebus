using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using System.Threading.Tasks;

namespace Moosesoft.Azure.ServiceBus
{
    public class DeferredMessagePlugin : ServiceBusPlugin
    {
        private readonly IMessageReceiver _messageReceiver;
        public override string Name => nameof(DeferredMessagePlugin);

        public DeferredMessagePlugin(IMessageReceiver messageReceiver)
        {
            _messageReceiver = messageReceiver;
        }

        public override Task<Message> AfterMessageReceive(Message message)
        {
            return _messageReceiver.GetDeferredMessageAsync(message);
        }
    }
}