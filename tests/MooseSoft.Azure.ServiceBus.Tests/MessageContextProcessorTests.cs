using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MooseSoft.Azure.ServiceBus.Abstractions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace MooseSoft.Azure.ServiceBus.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class MessageContextProcessorTests : MessageTestBase
    {
        private IMessageContextProcessor _sut;
        private IMessageProcessor _messageProcessor;
        private IFailurePolicy _failurePolicy;
        private IMessageReceiver _messageReceiver;
        private IMessageSender _messageSender;

        [TestInitialize]
        public void Init()
        {
            _messageReceiver = Substitute.For<IMessageReceiver>();
            _messageSender = Substitute.For<IMessageSender>();
            _failurePolicy = Substitute.For<IFailurePolicy>();
            _messageProcessor = Substitute.For<IMessageProcessor>();
            _sut = new MessageContextProcessor(_messageProcessor, _failurePolicy);
        }

        [TestMethod]
        public async Task ProcessMessageContextAsync_Test()
        {
            //Arrange
            var message = CreateMessage();
            var context = new TestMessageContext(message, _messageReceiver, _messageSender);

            //Act
            await _sut.ProcessMessageContextAsync(context, CancellationToken.None).ConfigureAwait(false);

            //Assert
            await _messageProcessor.Received().ProcessMessageAsync(Arg.Is(message), Arg.Is(CancellationToken.None))
                .ConfigureAwait(false);

            await _messageReceiver.Received().CompleteAsync(Arg.Is(message.SystemProperties.LockToken)).ConfigureAwait(false);
            await _messageReceiver.DidNotReceiveWithAnyArgs().AbandonAsync(null).ConfigureAwait(false);

        }

        [TestMethod]
        public async Task ProcessMessageContextAsync_Deferred_Success_Test()
        {
            //Arrange
            var message = CreateMessage();
            message.UserProperties.Add(Constants.DeferredKey, long.MaxValue);
            var deferredMessage = CreateMessage();
            _messageReceiver.ReceiveDeferredMessageAsync(Arg.Is(long.MaxValue)).Returns(deferredMessage);

            var context = new TestMessageContext(message, _messageReceiver, _messageSender);

            //Act
            await _sut.ProcessMessageContextAsync(context, CancellationToken.None).ConfigureAwait(false);

            //Assert
            await _messageProcessor.Received().ProcessMessageAsync(Arg.Is(deferredMessage), Arg.Is(CancellationToken.None))
                .ConfigureAwait(false);

            await _messageReceiver.Received().CompleteAsync(Arg.Is(message.SystemProperties.LockToken)).ConfigureAwait(false);
            await _messageReceiver.Received().CompleteAsync(Arg.Is(deferredMessage.SystemProperties.LockToken)).ConfigureAwait(false);
            await _messageReceiver.Received().ReceiveDeferredMessageAsync(Arg.Is(long.MaxValue)).ConfigureAwait(false);
            await _messageReceiver.DidNotReceiveWithAnyArgs().AbandonAsync(null).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task ProcessMessageContextAsync_Failed_CannotHandle_Test()
        {
            //Arrange
            var message = CreateMessage();
            var context = new TestMessageContext(message, _messageReceiver, _messageSender);
            _failurePolicy.CanHandle(Arg.Any<Exception>()).Returns(false);
            _messageProcessor.ProcessMessageAsync(Arg.Any<Message>(), Arg.Any<CancellationToken>())
                .Throws(new Exception());

            //Act
            await _sut.ProcessMessageContextAsync(context, CancellationToken.None).ConfigureAwait(false);

            //Assert
            await _messageProcessor.Received().ProcessMessageAsync(Arg.Is(message), Arg.Is(CancellationToken.None))
                .ConfigureAwait(false);

            await _messageReceiver.DidNotReceiveWithAnyArgs().CompleteAsync(null).ConfigureAwait(false);
            await _messageReceiver.Received().AbandonAsync(Arg.Is(message.SystemProperties.LockToken)).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task ProcessMessageContextAsync_FailurePolicy_Handle_Test()
        {
            //Arrange
            var message = CreateMessage();
            var context = new TestMessageContext(message, _messageReceiver, _messageSender);
            _failurePolicy.CanHandle(Arg.Any<Exception>()).Returns(true);
            _messageProcessor.ProcessMessageAsync(Arg.Any<Message>(), Arg.Any<CancellationToken>())
                .Throws(new Exception());

            //Act
            await _sut.ProcessMessageContextAsync(context, CancellationToken.None).ConfigureAwait(false);

            //Assert
            await _messageProcessor.Received().ProcessMessageAsync(Arg.Is(message), Arg.Is(CancellationToken.None))
                .ConfigureAwait(false);

            await _messageReceiver.DidNotReceiveWithAnyArgs().CompleteAsync(null).ConfigureAwait(false);
            await _messageReceiver.DidNotReceiveWithAnyArgs().AbandonAsync(null).ConfigureAwait(false);

            await _failurePolicy.HandleFailureAsync(Arg.Is(context), Arg.Is(CancellationToken.None))
                .ConfigureAwait(false);
        }
    }
}