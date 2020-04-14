using FluentAssertions;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MooseSoft.Azure.ServiceBus.Abstractions;
using MooseSoft.Azure.ServiceBus.BackOffDelayStrategy;
using MooseSoft.Azure.ServiceBus.FailurePolicy;
using MooseSoft.Azure.ServiceBus.MessagePump;
using NSubstitute;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace MooseSoft.Azure.ServiceBus.Tests.MessagePump
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class MessagePumpBuilderTests
    {
        private MessagePumpBuilder _sut;
        private IMessageReceiver _messageReceiver;

        [TestInitialize]
        public void Init()
        {
            _messageReceiver = Substitute.For<IMessageReceiver>();
            _sut = new MessagePumpBuilder(_messageReceiver);
        }

        [TestMethod]
        public void WithMessageProcessor_Test()
        {
            //Arrange

            //Act
            var result = _sut.WithMessageProcessor<TestMessageProcessor>();

            //Assert
            result.Should().NotBeNull();
            var state = (result as MessagePumpBuilder)?.BuilderState;
            state.Should().NotBeNull();
            // ReSharper disable once PossibleNullReferenceException
            state.MessageProcessor.Should().NotBeNull();
            state.MessageProcessor.Should().BeOfType<TestMessageProcessor>();
        }

        [TestMethod]
        public void WithCloneMessageFailurePolicy_Test()
        {
            //Arrange

            //Act
            var result = _sut.WithCloneMessageFailurePolicy();

            //Assert
            result.Should().NotBeNull();
            var state = (result as MessagePumpBuilder)?.BuilderState;
            state.Should().NotBeNull();
            // ReSharper disable once PossibleNullReferenceException
            state.FailurePolicy.Should().BeNull();
            state.FailurePolicyType.Should().Be<CloneMessageFailurePolicy>();
            state.CanHandle(new Exception()).Should().Be(true);
        }

        [TestMethod]
        public void WithDeferMessageFailurePolicy_Test()
        {
            //Arrange
            static bool CanHandle(Exception e) => e is InvalidOperationException;

            //Act
            var result = _sut.WithDeferMessageFailurePolicy(CanHandle);

            //Assert
            result.Should().NotBeNull();
            var state = (result as MessagePumpBuilder)?.BuilderState;
            state.Should().NotBeNull();
            // ReSharper disable once PossibleNullReferenceException
            state.FailurePolicy.Should().BeNull();
            state.FailurePolicyType.Should().Be<DeferMessageFailurePolicy>();
            state.CanHandle(new Exception()).Should().Be(false);
            state.CanHandle(new InvalidOperationException()).Should().Be(true);
        }

        [TestMethod]
        public void WithAbandonMessageFailurePolicy_Test()
        {
            //Arrange

            //Act
            var result = _sut.WithAbandonMessageFailurePolicy();

            //Assert
            result.Should().NotBeNull();
            var state = (result as MessagePumpBuilder)?.BuilderState;
            state.Should().NotBeNull();
            // ReSharper disable once PossibleNullReferenceException
            state.FailurePolicyType.Should().BeNull();
            state.FailurePolicy.Should().BeOfType<AbandonMessageFailurePolicy>();
            state.CanHandle.Should().BeNull();
        }

        [TestMethod]
        public void WithExponentialBackOffDelayStrategy_Test()
        {
            //Arrange

            //Act
            var result = _sut.WithExponentialBackOffDelayStrategy();

            //Assert
            result.Should().NotBeNull();
            var state = (result as MessagePumpBuilder)?.BuilderState;
            state.Should().NotBeNull();
            // ReSharper disable once PossibleNullReferenceException
            state.BackOffDelayStrategy.Should().BeOfType<ExponentialBackOffDelayStrategy>();
        }

        [TestMethod]
        public void WithExponentialBackOffDelayStrategy_MaxDelay_Test()
        {
            //Arrange
            var minute = TimeSpan.FromMinutes(1);

            //Act
            var result = _sut.WithExponentialBackOffDelayStrategy(minute);

            //Assert
            result.Should().NotBeNull();
            var state = (result as MessagePumpBuilder)?.BuilderState;
            state.Should().NotBeNull();
            // ReSharper disable once PossibleNullReferenceException
            state.BackOffDelayStrategy.Should().BeOfType<ExponentialBackOffDelayStrategy>();
            state.BackOffDelayStrategy.Calculate(10).Should().Be(minute);
        }

        [TestMethod]
        public void WithConstantBackOffDelayStrategy_Test()
        {
            //Arrange

            //Act
            var result = _sut.WithConstantBackOffDelayStrategy();

            //Assert
            result.Should().NotBeNull();
            var state = (result as MessagePumpBuilder)?.BuilderState;
            state.Should().NotBeNull();
            // ReSharper disable once PossibleNullReferenceException
            state.BackOffDelayStrategy.Should().BeOfType<ConstantBackOffDelayStrategy>();
        }

        [TestMethod]
        public void WithConstantBackOffDelayStrategy_SetDelayTime_Test()
        {
            //Arrange
            var minute = TimeSpan.FromMinutes(1);

            //Act
            var result = _sut.WithConstantBackOffDelayStrategy(minute);

            //Assert
            result.Should().NotBeNull();
            var state = (result as MessagePumpBuilder)?.BuilderState;
            state.Should().NotBeNull();
            // ReSharper disable once PossibleNullReferenceException
            state.BackOffDelayStrategy.Should().BeOfType<ConstantBackOffDelayStrategy>();
            state.BackOffDelayStrategy.Calculate(10).Should().Be(minute);
        }

        [TestMethod]
        public void WithLinearBackOffDelayStrategy_Test()
        {
            //Arrange

            //Act
            var result = _sut.WithLinearBackOffDelayStrategy();

            //Assert
            result.Should().NotBeNull();
            var state = (result as MessagePumpBuilder)?.BuilderState;
            state.Should().NotBeNull();
            // ReSharper disable once PossibleNullReferenceException
            state.BackOffDelayStrategy.Should().BeOfType<LinearBackOffDelayStrategy>();
        }

        [TestMethod]
        public void WithLinearBackOffDelayStrategy_SetDelayTime_Test()
        {
            //Arrange
            var minute = TimeSpan.FromMinutes(1);

            //Act
            var result = _sut.WithLinearBackOffDelayStrategy(minute);

            //Assert
            result.Should().NotBeNull();
            var state = (result as MessagePumpBuilder)?.BuilderState;
            state.Should().NotBeNull();
            // ReSharper disable once PossibleNullReferenceException
            state.BackOffDelayStrategy.Should().BeOfType<LinearBackOffDelayStrategy>();
            state.BackOffDelayStrategy.Calculate(10).Should().Be(10 * minute);
        }

        [TestMethod]
        public void WithZeroBackOffDelayStrategy_Test()
        {
            //Arrange

            //Act
            var result = _sut.WithZeroBackOffDelayStrategy();

            //Assert
            result.Should().NotBeNull();
            var state = (result as MessagePumpBuilder)?.BuilderState;
            state.Should().NotBeNull();
            // ReSharper disable once PossibleNullReferenceException
            state.BackOffDelayStrategy.Should().BeOfType<ZeroBackOffDelayStrategy>();
        }

        [TestMethod]
        public void WithBackOffDelayStrategy_Test()
        {
            //Arrange

            //Act
            var result = _sut.WithBackOffDelayStrategy<TestBackOffDelayStrategy>();

            //Assert
            result.Should().NotBeNull();
            var state = (result as MessagePumpBuilder)?.BuilderState;
            state.Should().NotBeNull();
            // ReSharper disable once PossibleNullReferenceException
            state.BackOffDelayStrategy.Should().BeOfType<TestBackOffDelayStrategy>();
        }

        [TestMethod]
        public void BuildMessagePump_Test()
        {
            //Arrange
            _sut.BuilderState.MessageProcessor = Substitute.For<IMessageProcessor>();
            _sut.BuilderState.FailurePolicy = Substitute.For<IFailurePolicy>();

            var options = new MessagePumpBuilderOptions(args => Task.CompletedTask)
            {
                MaxAutoRenewDuration = TimeSpan.FromMinutes(50),
                MaxConcurrentCalls = 10, 
                ShouldCompleteOnException = exception => false
            };

            //Act
            var result = _sut.BuildMessagePump(options);

            //Assert
            result.Should().NotBeNull();
            result.ReceivedWithAnyArgs()
                .RegisterMessageHandler(Arg.Any<Func<Message, CancellationToken, Task>>(), Arg.Any<MessageHandlerOptions>());
        }

        [TestMethod]
        public void BuildMessagePump_ArgumentNullException_Test()
        {
            //Arrange
            _sut.BuilderState.MessageProcessor = Substitute.For<IMessageProcessor>();
            _sut.BuilderState.FailurePolicy = Substitute.For<IFailurePolicy>();

            //Act
            Action act = () => _sut.BuildMessagePump(null);

            //Assert
            act.Should().ThrowExactly<ArgumentNullException>();
            _sut.BuilderState.MessageReceiver.DidNotReceiveWithAnyArgs()
                .RegisterMessageHandler(Arg.Any<Func<Message, CancellationToken, Task>>(), Arg.Any<MessageHandlerOptions>());
        }

        [TestMethod]
        public void BuildMessagePump_CloneMessageFailurePolicy_Test()
        {
            //Arrange
            _sut.BuilderState.MessageProcessor = Substitute.For<IMessageProcessor>();
            _sut.BuilderState.FailurePolicyType = typeof(CloneMessageFailurePolicy);
            _sut.BuilderState.CanHandle = MessagePumpBuilder.DefaultCanHandle;

            //Act
            var result = _sut.BuildMessagePump(args => Task.CompletedTask);

            //Assert
            result.Should().NotBeNull();
            result.ReceivedWithAnyArgs()
                .RegisterMessageHandler(Arg.Any<Func<Message, CancellationToken, Task>>(), Arg.Any<MessageHandlerOptions>());
        }

        class TestMessageProcessor : IMessageProcessor
        {
            public Task ProcessMessageAsync(Message message, CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }
        }

        class TestBackOffDelayStrategy : IBackOffDelayStrategy
        {
            public TimeSpan Calculate(int attempts)
            {
                return TimeSpan.Zero;
            }
        }
    }
}