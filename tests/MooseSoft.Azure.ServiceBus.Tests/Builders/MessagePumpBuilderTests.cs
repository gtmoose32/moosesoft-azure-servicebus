﻿using FluentAssertions;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MooseSoft.Azure.ServiceBus.Abstractions;
using MooseSoft.Azure.ServiceBus.BackOffDelayStrategy;
using MooseSoft.Azure.ServiceBus.Builders;
using MooseSoft.Azure.ServiceBus.FailurePolicy;
using NSubstitute;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MooseSoft.Azure.ServiceBus.Tests.Support;

namespace MooseSoft.Azure.ServiceBus.Tests.Builders
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
            var state = (result as MessagePumpBuilder)?.GetBuilderState();
            state.Should().NotBeNull();
            // ReSharper disable once PossibleNullReferenceException
            state.MessageProcessor.Should().NotBeNull();
            state.MessageProcessor.Should().BeOfType<TestMessageProcessor>();
        }

        [TestMethod]
        public void WithMessageProcessor_Func_Test()
        {
            //Arrange

            //Act
            var result = _sut.WithMessageProcessor((message, token) => Task.CompletedTask);

            //Assert
            result.Should().NotBeNull();
            var state = (result as MessagePumpBuilder)?.GetBuilderState();
            state.Should().NotBeNull();
            // ReSharper disable once PossibleNullReferenceException
            state.MessageProcessor.Should().NotBeNull();
            state.MessageProcessor.Should().BeOfType<DefaultMessageProcessor>();
        }

        [TestMethod]
        public void WithCloneMessageFailurePolicy_Test()
        {
            //Arrange

            //Act
            var result = _sut.WithCloneMessageFailurePolicy();

            //Assert
            result.Should().NotBeNull();
            var state = (result as MessagePumpBuilder)?.GetBuilderState();
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
            var state = (result as MessagePumpBuilder)?.GetBuilderState();
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
            var state = (result as MessagePumpBuilder)?.GetBuilderState();
            state.Should().NotBeNull();
            // ReSharper disable once PossibleNullReferenceException
            state.FailurePolicyType.Should().BeNull();
            state.FailurePolicy.Should().BeOfType<AbandonMessageFailurePolicy>();
            state.CanHandle.Should().BeNull();
        }

        [TestMethod]
        public void WithFailurePolicy_Test()
        {
            //Arrange

            //Act
            var result = _sut.WithFailurePolicy(new TestFailurePolicy());

            //Assert
            result.Should().NotBeNull();
            var state = (result as MessagePumpBuilder)?.GetBuilderState();
            state.Should().NotBeNull();
            // ReSharper disable once PossibleNullReferenceException
            state.FailurePolicyType.Should().BeNull();
            state.FailurePolicy.Should().BeOfType<TestFailurePolicy>();
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
            var state = (result as MessagePumpBuilder)?.GetBuilderState();
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
            var state = (result as MessagePumpBuilder)?.GetBuilderState();
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
            var state = (result as MessagePumpBuilder)?.GetBuilderState();
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
            var state = (result as MessagePumpBuilder)?.GetBuilderState();
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
            var state = (result as MessagePumpBuilder)?.GetBuilderState();
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
            var state = (result as MessagePumpBuilder)?.GetBuilderState();
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
            var state = (result as MessagePumpBuilder)?.GetBuilderState();
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
            var state = (result as MessagePumpBuilder)?.GetBuilderState();
            state.Should().NotBeNull();
            // ReSharper disable once PossibleNullReferenceException
            state.BackOffDelayStrategy.Should().BeOfType<TestBackOffDelayStrategy>();
        }

        [TestMethod]
        public void BuildMessagePump_Test()
        {
            //Arrange
            _sut.GetBuilderState().MessageProcessor = Substitute.For<IMessageProcessor>();
            _sut.GetBuilderState().FailurePolicy = Substitute.For<IFailurePolicy>();

            //Act
            var result = _sut.Build();

            //Assert
            result.Should().NotBeNull();
            result.ReceivedWithAnyArgs()
                .RegisterMessageHandler(Arg.Any<Func<Message, CancellationToken, Task>>(), Arg.Any<MessageHandlerOptions>());
        }

        [TestMethod]
        public void BuildMessagePump_CloneMessageFailurePolicy_Test()
        {
            //Arrange
            _sut.GetBuilderState().MessageProcessor = Substitute.For<IMessageProcessor>();
            _sut.GetBuilderState().FailurePolicyType = typeof(CloneMessageFailurePolicy);
            _sut.GetBuilderState().CanHandle = MessagePumpBuilder.DefaultCanHandle;

            //Act
            var result = _sut.Build();

            //Assert
            result.Should().NotBeNull();
            result.ReceivedWithAnyArgs()
                .RegisterMessageHandler(Arg.Any<Func<Message, CancellationToken, Task>>(), Arg.Any<MessageHandlerOptions>());
        }
    }
}