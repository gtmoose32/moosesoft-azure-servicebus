using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MooseSoft.Azure.ServiceBus.BackOffDelayStrategy;
using System;
using System.Diagnostics.CodeAnalysis;

namespace MooseSoft.Azure.ServiceBus.Tests.BackOffDelayStrategy
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class LinearBackOffDelayStrategyTests
    {
        [TestMethod]
        public void Calculate_Test()
        {
            //Arrange
            var sut = LinearBackOffDelayStrategy.Default;

            //Act
            var total = TimeSpan.Zero;
            for (var i = 1; i <= 10; i++)
            {
                total += sut.Calculate(i);
            }
            
            //Assert
            total.TotalMinutes.Should().Be(55);
        }
    }
}