using FluentAssertions;
using Microsoft.Azure.ServiceBus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;

namespace MooseSoft.Azure.ServiceBus.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class MessageExtensionsTests
    {
        [TestMethod]
        public void TryGetDeferredSequenceNumber_NullCorrelationId_Test()
        {
            //Arrange
            var sut = new Message();

            //Act
            var result = sut.TryGetDeferredSequenceNumber(out _);

            //Assert
            result.Should().BeFalse();
        }
    }
}