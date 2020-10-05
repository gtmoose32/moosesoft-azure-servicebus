using FluentAssertions;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moosesoft.Azure.ServiceBus.Builders;
using NSubstitute;
using System.Diagnostics.CodeAnalysis;

namespace Moosesoft.Azure.ServiceBus.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class BuilderTests
    {
        [TestMethod]
        public void MessageContextProcessor_Configure_Test()
        {
            //Act
            var result = Builder.ConfigureMessageContextProcessor();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<MessageContextProcessorBuilder>();
        }

        [TestMethod]
        public void MessagePump_Configure_Test()
        {
            //Act
            var result = Builder.ConfigureMessagePump(Substitute.For<IMessageReceiver>());

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<MessagePumpBuilder>();
        }
    }
}