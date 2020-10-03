using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moosesoft.Azure.ServiceBus.Abstractions;
using Moosesoft.Azure.ServiceBus.FailurePolicy;
using NSubstitute;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Moosesoft.Azure.ServiceBus.Tests.FailurePolicy
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class DeferMessageFailurePolicyTests : MessageTestBase
    {
        private IFailurePolicy _sut;

        [TestInitialize]
        public void Init()
        {
            _sut = new DeferMessageFailurePolicy(ex => true);
        }

        [TestMethod]
        public async Task HandleFailureAsync_Test()
        {
            //Arrange
            var receiver = Substitute.For<IMessageReceiver>();
            var sender = Substitute.For<IMessageSender>();

            var message = CreateMessage();

            //Act
            await _sut.HandleFailureAsync(new TestMessageContext(message, receiver, sender), CancellationToken.None);

            //Assert
            await receiver.Received().DeferAsync(Arg.Is(message.SystemProperties.LockToken)).ConfigureAwait(false);
            await sender.SendAsync(Arg.Any<Message>()).ConfigureAwait(false);
            await receiver.DidNotReceiveWithAnyArgs().DeadLetterAsync(null).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task HandleFailureAsync_MaxDeliveryCount_Test()
        {
            //Arrange
            var receiver = Substitute.For<IMessageReceiver>();
            var sender = Substitute.For<IMessageSender>();

            var message = CreateMessage(10);

            //Act
            await _sut.HandleFailureAsync(new TestMessageContext(message, receiver, sender), CancellationToken.None);

            //Assert
            await receiver.DidNotReceiveWithAnyArgs().DeferAsync(null).ConfigureAwait(false);
            await sender.DidNotReceiveWithAnyArgs().SendAsync(Arg.Any<Message>()).ConfigureAwait(false);
            await receiver.Received().AbandonAsync(Arg.Is(message.SystemProperties.LockToken)).ConfigureAwait(false);
        }
    }
}