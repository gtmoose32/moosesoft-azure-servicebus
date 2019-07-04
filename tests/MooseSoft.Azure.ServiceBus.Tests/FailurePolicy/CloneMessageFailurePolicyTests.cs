using Microsoft.Azure.ServiceBus.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MooseSoft.Azure.ServiceBus.Abstractions;
using MooseSoft.Azure.ServiceBus.FailurePolicy;
using NSubstitute;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace MooseSoft.Azure.ServiceBus.Tests.FailurePolicy
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class CloneMessageFailurePolicyTests : MessageTestBase
    {
        private IFailurePolicy _sut;

        [TestInitialize]
        public void Init()
        {
            _sut = new CloneMessageFailurePolicy(ex => ex is InvalidOperationException);
        }

        [TestMethod]
        public void CanHandle_Test()
        {
            //Act
            var result = _sut.CanHandle(new Exception());
            var result2 = _sut.CanHandle(new InvalidOperationException());

            //Assert
            Assert.IsFalse(result);
            Assert.IsTrue(result2);
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
            await receiver.Received().CompleteAsync(Arg.Is(message.SystemProperties.LockToken)).ConfigureAwait(false);
            await sender.SendAsync(Arg.Any<Message>()).ConfigureAwait(false);
            await receiver.DidNotReceiveWithAnyArgs().DeadLetterAsync(null).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task HandleFailureAsync_MaxDeliveryCount_Test()
        {
            //Arrange
            var receiver = Substitute.For<IMessageReceiver>();
            var sender = Substitute.For<IMessageSender>();

            var message = CreateMessage();
            message.UserProperties.Add(Constants.RetryCountKey, 9);

            //Act
            await _sut.HandleFailureAsync(new TestMessageContext(message, receiver, sender), CancellationToken.None);

            //Assert
            await receiver.DidNotReceiveWithAnyArgs().CompleteAsync(null).ConfigureAwait(false);
            await sender.DidNotReceiveWithAnyArgs().SendAsync(Arg.Any<Message>()).ConfigureAwait(false);
            await receiver.Received().DeadLetterAsync(Arg.Is(message.SystemProperties.LockToken), Arg.Any<string>())
                .ConfigureAwait(false);
        }
    }
}