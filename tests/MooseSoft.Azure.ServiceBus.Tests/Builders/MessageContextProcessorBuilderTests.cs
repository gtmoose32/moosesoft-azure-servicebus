using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moosesoft.Azure.ServiceBus.Abstractions;
using Moosesoft.Azure.ServiceBus.Builders;
using Moosesoft.Azure.ServiceBus.FailurePolicy;
using Moosesoft.Azure.ServiceBus.Tests.Support;
using NSubstitute;
using System.Diagnostics.CodeAnalysis;

namespace Moosesoft.Azure.ServiceBus.Tests.Builders
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class MessageContextProcessorBuilderTests
    {
        [TestMethod]
        public void WithBackOffDelayStrategy_Test()
        {
            //Arrange
             var sut = new MessageContextProcessorBuilder();

            //Act
            var result = sut.WithBackOffDelayStrategy<TestBackOffDelayStrategy>();

            //Assert
            result.Should().NotBeNull();
            var state = (result as MessageContextProcessorBuilder)?.GetBuilderState();
            state.Should().NotBeNull();
            // ReSharper disable once PossibleNullReferenceException
            state.BackOffDelayStrategy.Should().BeOfType<TestBackOffDelayStrategy>();
        }

        [TestMethod]
        public void WithAbandonMessageFailurePolicy_Test()
        {
            //Arrange
            var sut = new MessageContextProcessorBuilder();

            //Act
            var result = sut.WithAbandonMessageFailurePolicy();

            //Assert
            result.Should().NotBeNull();
            var state = (result as MessageContextProcessorBuilder)?.GetBuilderState();
            state.Should().NotBeNull();
            // ReSharper disable once PossibleNullReferenceException
            state.FailurePolicy.Should().BeOfType<AbandonMessageFailurePolicy>();
        }

        [TestMethod]
        public void WithFailurePolicy_Test()
        {
            //Arrange
            var sut = new MessageContextProcessorBuilder();

            //Act
            var result = sut.WithFailurePolicy(new TestFailurePolicy());

            //Assert
            result.Should().NotBeNull();
            var state = (result as MessageContextProcessorBuilder)?.GetBuilderState();
            state.Should().NotBeNull();
            // ReSharper disable once PossibleNullReferenceException
            state.FailurePolicy.Should().BeOfType<TestFailurePolicy>();
        }

        [TestMethod]
        public void Build_Test()
        {
            //Arrange
            var sut = new MessageContextProcessorBuilder();
            sut.GetBuilderState().MessageProcessor = Substitute.For<IMessageProcessor>();
            sut.GetBuilderState().FailurePolicy = Substitute.For<IFailurePolicy>();

            //Act
            var result = sut.Build();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<MessageContextProcessor>();
        }
    }
}