using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moosesoft.Azure.ServiceBus.Abstractions;
using Moosesoft.Azure.ServiceBus.FailurePolicy;
using NSubstitute;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Moosesoft.Azure.ServiceBus.Tests.FailurePolicy
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class AbandonMessageFailurePolicyTests
    {
        private IFailurePolicy _sut;

        [TestInitialize]
        public void Init()
        {
            _sut = new AbandonMessageFailurePolicy();
        }

        [TestMethod]
        public void CanHandle_Test()
        {
            //Act
            var result = _sut.CanHandle(new Exception());

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task HandleFailureAsync_Test()
        {
            //Arrange
            var receiver = Substitute.For<IMessageReceiver>();

            //Act
            await _sut.HandleFailureAsync(new MessageContext(new Message(), receiver), CancellationToken.None);

            //Assert
            await receiver.DidNotReceiveWithAnyArgs().CompleteAsync(null).ConfigureAwait(false);
            await receiver.DidNotReceiveWithAnyArgs().DeferAsync(null).ConfigureAwait(false);
            await receiver.DidNotReceiveWithAnyArgs().AbandonAsync(null).ConfigureAwait(false);
            await receiver.DidNotReceiveWithAnyArgs().DeadLetterAsync(null).ConfigureAwait(false);
        }
    }
}