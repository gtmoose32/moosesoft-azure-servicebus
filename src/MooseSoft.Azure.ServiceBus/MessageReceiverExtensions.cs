using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using System.Threading.Tasks;
using System.Transactions;

namespace MooseSoft.Azure.ServiceBus
{
    public static class MessageReceiverExtensions
    {
        public static IMessageReceiver AddDeferredMessagePlugin(this IMessageReceiver messageReceiver)
        {
            messageReceiver.RegisterPlugin(new DeferredMessagePlugin(messageReceiver));

            return messageReceiver;
        }

        public static async Task<Message> GetDeferredMessageAsync(this IMessageReceiver messageReceiver, Message message)
        {
            var sequenceNumber = message.GetDeferredSequenceNumber();
            if (!sequenceNumber.HasValue) return message;

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await messageReceiver.CompleteAsync(message.SystemProperties.LockToken).ConfigureAwait(false);
                message = await messageReceiver.ReceiveDeferredMessageAsync(sequenceNumber.Value)
                    .ConfigureAwait(false);

                scope.Complete();
            }

            return message;
        }
    }
}