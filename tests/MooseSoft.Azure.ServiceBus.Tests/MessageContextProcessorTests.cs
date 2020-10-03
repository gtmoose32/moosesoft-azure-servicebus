using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MooseSoft.Azure.ServiceBus.Abstractions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
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
            _sut = new MessageContextProcessor(_messageProcessor, _failurePolicy, e => e is InvalidOperationException);
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
            await _messageReceiver.DidNotReceiveWithAnyArgs().ReceiveDeferredMessageAsync(Arg.Any<long>()).ConfigureAwait(false);
            await _messageReceiver.DidNotReceiveWithAnyArgs().AbandonAsync(Arg.Any<string>()).ConfigureAwait(false);

            _failurePolicy.DidNotReceiveWithAnyArgs().CanHandle(Arg.Any<Exception>());
            await _failurePolicy.DidNotReceiveWithAnyArgs()
                .HandleFailureAsync(Arg.Any<MessageContext>(), Arg.Any<CancellationToken>()).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task ProcessMessageContextAsync_FuncMessageProcessor_Test()
        {
            //Arrange
            var message = CreateMessage();
            var context = new TestMessageContext(message, _messageReceiver, _messageSender);

            var processor = Substitute.For<Func<Message, CancellationToken, Task>>();
            processor(Arg.Any<Message>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

            var sut = new MessageContextProcessor(processor, _failurePolicy, e => true);

            //Act
            await sut.ProcessMessageContextAsync(context, CancellationToken.None).ConfigureAwait(false);

            //Assert
            await processor.Received()(Arg.Is(message), Arg.Is(CancellationToken.None))
                .ConfigureAwait(false);

            await _messageReceiver.Received().CompleteAsync(Arg.Is(message.SystemProperties.LockToken)).ConfigureAwait(false);
            await _messageReceiver.DidNotReceiveWithAnyArgs().ReceiveDeferredMessageAsync(Arg.Any<long>()).ConfigureAwait(false);
            await _messageReceiver.DidNotReceiveWithAnyArgs().AbandonAsync(Arg.Any<string>()).ConfigureAwait(false);

            _failurePolicy.DidNotReceiveWithAnyArgs().CanHandle(Arg.Any<Exception>());
            await _failurePolicy.DidNotReceiveWithAnyArgs()
                .HandleFailureAsync(Arg.Any<MessageContext>(), Arg.Any<CancellationToken>()).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task ProcessMessageContextAsync_Deferred_Success_Test()
        {
            //Arrange
            var message = CreateMessage();
            message.Label = Constants.DeferredKey;
            message.CorrelationId = long.MaxValue.ToString();

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

            _failurePolicy.DidNotReceiveWithAnyArgs().CanHandle(Arg.Any<Exception>());
            await _failurePolicy.DidNotReceiveWithAnyArgs()
                .HandleFailureAsync(Arg.Any<MessageContext>(), Arg.Any<CancellationToken>()).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task ProcessMessageContextAsync_Deferred_UsePlugin_Success_Test()
        {
            //Arrange
            var message = CreateMessage();
            _messageReceiver.RegisteredPlugins.Returns(
                new List<ServiceBusPlugin>
                {
                    new DeferredMessagePlugin(_messageReceiver)
                });

            var context = new TestMessageContext(message, _messageReceiver, _messageSender);

            //Act
            await _sut.ProcessMessageContextAsync(context, CancellationToken.None).ConfigureAwait(false);

            //Assert
            await _messageProcessor.Received().ProcessMessageAsync(Arg.Is(message), Arg.Is(CancellationToken.None))
                .ConfigureAwait(false);

            await _messageReceiver.Received().CompleteAsync(Arg.Is(message.SystemProperties.LockToken)).ConfigureAwait(false);
            await _messageReceiver.DidNotReceiveWithAnyArgs().ReceiveDeferredMessageAsync(Arg.Any<long>()).ConfigureAwait(false);
            await _messageReceiver.DidNotReceiveWithAnyArgs().AbandonAsync(null).ConfigureAwait(false);

            _failurePolicy.DidNotReceiveWithAnyArgs().CanHandle(Arg.Any<Exception>());
            await _failurePolicy.DidNotReceiveWithAnyArgs()
                .HandleFailureAsync(Arg.Any<MessageContext>(), Arg.Any<CancellationToken>()).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task ProcessMessageContextAsync_Failed_Complete_Test()
        {
            //Arrange
            var message = CreateMessage();
            var context = new TestMessageContext(message, _messageReceiver, _messageSender);
            _messageProcessor.ProcessMessageAsync(Arg.Any<Message>(), Arg.Any<CancellationToken>())
                .Throws(new InvalidOperationException());

            //Act
            await _sut.ProcessMessageContextAsync(context, CancellationToken.None).ConfigureAwait(false);

            //Assert
            await _messageProcessor.Received().ProcessMessageAsync(Arg.Is(message), Arg.Is(CancellationToken.None))
                .ConfigureAwait(false);

            await _messageReceiver.Received().CompleteAsync(Arg.Is(message.SystemProperties.LockToken)).ConfigureAwait(false);
            await _messageReceiver.DidNotReceiveWithAnyArgs().AbandonAsync(Arg.Any<string>()).ConfigureAwait(false);

            _failurePolicy.DidNotReceiveWithAnyArgs().CanHandle(Arg.Any<Exception>());
            await _failurePolicy.DidNotReceiveWithAnyArgs()
                .HandleFailureAsync(Arg.Any<MessageContext>(), Arg.Any<CancellationToken>()).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task ProcessMessageContextAsync_Failed_Abandon_Test()
        {
            //Arrange
            var message = CreateMessage();
            var context = new TestMessageContext(message, _messageReceiver, _messageSender);
            _failurePolicy.CanHandle(Arg.Any<Exception>()).Returns(false);
            var exception = new Exception();
            _messageProcessor.ProcessMessageAsync(Arg.Any<Message>(), Arg.Any<CancellationToken>())
                .Throws(exception);

            //Act
            await _sut.ProcessMessageContextAsync(context, CancellationToken.None).ConfigureAwait(false);

            //Assert
            await _messageProcessor.Received().ProcessMessageAsync(Arg.Is(message), Arg.Is(CancellationToken.None))
                .ConfigureAwait(false);

            await _messageReceiver.DidNotReceiveWithAnyArgs().CompleteAsync(null).ConfigureAwait(false);
            await _messageReceiver.Received().AbandonAsync(Arg.Is(message.SystemProperties.LockToken)).ConfigureAwait(false);

            _failurePolicy.Received().CanHandle(Arg.Is(exception));
            await _failurePolicy.DidNotReceiveWithAnyArgs()
                .HandleFailureAsync(Arg.Any<MessageContext>(), Arg.Any<CancellationToken>()).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task ProcessMessageContextAsync_FailurePolicy_Handle_Test()
        {
            //Arrange
            var message = CreateMessage();
            var context = new TestMessageContext(message, _messageReceiver, _messageSender);
            _failurePolicy.CanHandle(Arg.Any<Exception>()).Returns(true);
            var exception = new Exception();
            _messageProcessor.ProcessMessageAsync(Arg.Any<Message>(), Arg.Any<CancellationToken>())
                .Throws(exception);

            //Act
            await _sut.ProcessMessageContextAsync(context, CancellationToken.None).ConfigureAwait(false);

            //Assert
            await _messageProcessor.Received().ProcessMessageAsync(Arg.Is(message), Arg.Is(CancellationToken.None))
                .ConfigureAwait(false);

            await _messageReceiver.DidNotReceiveWithAnyArgs().CompleteAsync(null).ConfigureAwait(false);
            await _messageReceiver.DidNotReceiveWithAnyArgs().AbandonAsync(null).ConfigureAwait(false);

            _failurePolicy.Received().CanHandle(Arg.Is(exception));
            await _failurePolicy.Received().HandleFailureAsync(Arg.Is(context), Arg.Is(CancellationToken.None))
                .ConfigureAwait(false);
        }
    }
}