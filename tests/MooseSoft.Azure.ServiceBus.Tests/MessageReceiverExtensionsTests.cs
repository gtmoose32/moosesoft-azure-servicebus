using FluentAssertions;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MooseSoft.Azure.ServiceBus.Builders;
using NSubstitute;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace MooseSoft.Azure.ServiceBus.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class MessageReceiverExtensionsTests
    {
        [TestMethod]
        public void AddDeferredMessagePlugin_Test()
        {
            //Arrange
            var sut = Substitute.For<IMessageReceiver>();

            //Act
            sut.AddDeferredMessagePlugin();

            //Assert
            sut.Received().RegisterPlugin(Arg.Any<DeferredMessagePlugin>());
        }

        [TestMethod]
        public async Task GetDeferredMessageAsync_DeferredSequenceNumber_NotFound_Test()
        {
            //Arrange
            var message = new Message();
            var sut = Substitute.For<IMessageReceiver>();

            //Act
            var result = await sut.GetDeferredMessageAsync(message);

            //Assert
            result.Should().NotBeNull();
            result.Should().Be(message);
            await sut.DidNotReceiveWithAnyArgs().CompleteAsync(Arg.Any<string>()).ConfigureAwait(false);
            await sut.DidNotReceiveWithAnyArgs().ReceiveDeferredMessageAsync(Arg.Any<long>()).ConfigureAwait(false);
        }

        [TestMethod]
        public void ConfigureMessagePump_Test()
        {
            //Arrange
            var sut = Substitute.For<IMessageReceiver>();

            //Act
            var result = sut.ConfigureMessagePump();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<MessagePumpBuilder>();
            var state = (result as MessagePumpBuilder)?.GetBuilderState();
            // ReSharper disable once PossibleNullReferenceException
            state.MessageReceiver.Should().Be(sut);
        }
    }
}