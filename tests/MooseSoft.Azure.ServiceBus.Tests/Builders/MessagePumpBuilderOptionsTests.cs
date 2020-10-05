using FluentAssertions;
using Microsoft.Azure.ServiceBus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moosesoft.Azure.ServiceBus.Builders;
using NSubstitute;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Moosesoft.Azure.ServiceBus.Tests.Builders
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class MessagePumpBuilderOptionsTests
    {
        [TestMethod]
        public void ImplicitOperator_Test()
        {
            //Arrange
            var maxRenew = TimeSpan.FromMinutes(10);
            const int concurrentCalls = 10;

            var exceptionReceivedHandler = Substitute.For<Func<ExceptionReceivedEventArgs, Task>>();
            var shouldCompleteOn = Substitute.For<Func<Exception, bool>>();
            var sut = new MessagePumpBuilderOptions(exceptionReceivedHandler)
            {
                MaxAutoRenewDuration = maxRenew,
                MaxConcurrentCalls = concurrentCalls, 
                ShouldCompleteOnException = shouldCompleteOn
            };

            //Act
            MessageHandlerOptions result = sut;

            //Assert
            sut.ShouldCompleteOnException.Should().Be(shouldCompleteOn);

            result.Should().NotBeNull();
            result.AutoComplete.Should().BeFalse();
            result.MaxAutoRenewDuration.Should().Be(maxRenew);
            result.MaxConcurrentCalls.Should().Be(concurrentCalls);
        }

        [TestMethod]
        public void ImplicitOperator_Null_Test()
        {
            //Arrange
            var maxRenew = TimeSpan.FromMinutes(5);
            const int concurrentCalls = 1;

            MessagePumpBuilderOptions sut = null;

            //Act
            // ReSharper disable once ExpressionIsAlwaysNull
            MessageHandlerOptions result = sut;

            //Assert
            result.Should().NotBeNull();
            result.ExceptionReceivedHandler.Should().NotBeNull();
            result.AutoComplete.Should().BeFalse();
            result.MaxAutoRenewDuration.Should().Be(maxRenew);
            result.MaxConcurrentCalls.Should().Be(concurrentCalls);
        }
    }
}