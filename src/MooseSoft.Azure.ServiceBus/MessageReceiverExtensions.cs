using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using System.Threading.Tasks;
using System.Transactions;

namespace MooseSoft.Azure.ServiceBus
{
    /// <summary>
    /// Class that defines extension methods for Azure Service Bus <see cref="IMessageReceiver"/>
    /// </summary>
    public static class MessageReceiverExtensions
    {
        /// <summary>
        /// Adds a <see cref="DeferredMessagePlugin"/> to the <see cref="IMessageReceiver"/> list of registered plugins.  This will handle any deferred message failure policy actions.
        /// </summary>
        /// <param name="messageReceiver">The message receiver to register the <see cref="ServiceBusPlugin"/> to.</param>
        /// <returns><see cref="IMessageReceiver"/></returns>
        public static IMessageReceiver AddDeferredMessagePlugin(this IMessageReceiver messageReceiver)
        {
            messageReceiver.RegisterPlugin(new DeferredMessagePlugin(messageReceiver));

            return messageReceiver;
        }

        internal static async Task<Message> GetDeferredMessageAsync(this IMessageReceiver messageReceiver, Message message)
        {
            if (!message.TryGetDeferredSequenceNumber(out var sequenceNumber)) return message;

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await messageReceiver.CompleteAsync(message.SystemProperties.LockToken).ConfigureAwait(false);
                message = await messageReceiver.ReceiveDeferredMessageAsync(sequenceNumber)
                    .ConfigureAwait(false);

                scope.Complete();
            }

            return message;
        }
    }
}