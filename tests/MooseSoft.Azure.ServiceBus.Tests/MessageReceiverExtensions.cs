using Microsoft.Azure.ServiceBus.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Diagnostics.CodeAnalysis;

namespace MooseSoft.Azure.ServiceBus.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class MessageReceiverExtensionsTests
    {
        [TestMethod]
        public void AddDeferredMessagePlugin_Test()
        {
            //Arrange
            var sut = Substitute.For<IMessageReceiver>();

            //Act
            sut.AddDeferredMessagePlugin();

            //Assert
            sut.Received().RegisterPlugin(Arg.Any<DeferredMessagePlugin>());
        }

        //public static IMessageReceiver AddDeferredMessagePlugin(this IMessageReceiver messageReceiver)
        //{
        //    messageReceiver.RegisterPlugin(new DeferredMessagePlugin(messageReceiver));

        //    return messageReceiver;
        //}

        //public static async Task<Message> GetDeferredMessageAsync(this IMessageReceiver messageReceiver, Message message)
        //{
        //    var sequenceNumber = message.GetDeferredSequenceNumber();
        //    if (!sequenceNumber.HasValue) return message;

        //    using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        //    {
        //        await messageReceiver.CompleteAsync(message.SystemProperties.LockToken).ConfigureAwait(false);
        //        message = await messageReceiver.ReceiveDeferredMessageAsync(sequenceNumber.Value)
        //            .ConfigureAwait(false);

        //        scope.Complete();
        //    }

        //    return message;
        //}
    }
}