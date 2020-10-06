using FluentAssertions;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Moosesoft.Azure.ServiceBus.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class DeferredMessagePluginTests : MessageTestBase
    {
        [TestMethod]
        public async Task AfterMessageReceive_Test()
        {
            //Arrange
            var receiver = Substitute.For<IMessageReceiver>();
            var message = CreateMessage();
            message.Label = Constants.DeferredKey;
            message.CorrelationId = long.MaxValue.ToString();

            var deferredMessage = CreateMessage();
            receiver.ReceiveDeferredMessageAsync(long.MaxValue).Returns(deferredMessage);

            var sut = new DeferredMessagePlugin(receiver);

            //Act
            var result = await sut.AfterMessageReceive(message).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            result.MessageId.Should().NotBeNullOrWhiteSpace()
                .And
                .Be(deferredMessage.MessageId);

            result.SystemProperties.Should().NotBeNull();
            result.SystemProperties.LockToken.Should().NotBeNullOrWhiteSpace()
                .And
                .Be(deferredMessage.SystemProperties.LockToken);

            await receiver.Received().CompleteAsync(Arg.Is(message.SystemProperties.LockToken)).ConfigureAwait(false);
            await receiver.Received().ReceiveDeferredMessageAsync(Arg.Is(long.MaxValue)).ConfigureAwait(false);
        }

        [TestMethod]
        public void Name_Test()
        {
            //Arrange
            var receiver = Substitute.For<IMessageReceiver>();
            var sut = new DeferredMessagePlugin(receiver);

            //Act
            var result = sut.Name;

            //Assert
            result.Should().NotBeNull();
            result.Should().Be(nameof(DeferredMessagePlugin));
        }
    }
}