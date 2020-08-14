using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MooseSoft.Azure.ServiceBus.BackOffDelayStrategy;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MooseSoft.Azure.ServiceBus.Tests.BackOffDelayStrategy
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ExponentialBackOffDelayStrategyTests
    {
        [TestMethod]
        public void Calculate_Test()
        {
            //Arrange
            var sut = ExponentialBackOffDelayStrategy.Default;
            var results = new List<TimeSpan>();

            //Act
            for (var i = 1; i <= 10; i++)
            {
                results.Add(sut.Calculate(i));
            }
            
            //Assert
            var hour = TimeSpan.FromHours(1);
            for (var i = 0; i < 10; i++)
            {
                if (i < 5)
                    Assert.IsTrue(TimeSpan.Zero < results[i] && hour > results[i]);
                else
                    Assert.IsTrue(hour == results[i]);
            }
        }

        [TestMethod]
        [DataRow(10, new[] { 10, 40, 90, 160, 250, 360 })]
        [DataRow(100, new[] { 100, 400, 900, 1600, 2500, 3600 })]
        public void Calculate_CustomInitialBackoffDelaySeconds_Test(int initialDelay, int[] delays)
        {
            // Arrange
            var sut = new ExponentialBackOffDelayStrategy(TimeSpan.FromHours(1), TimeSpan.FromSeconds(initialDelay));
            var results = new List<int>();

            // Act
            for (var i = 1; i <= 6; i++)
            {
                results.Add((int)sut.Calculate(i).TotalSeconds);
            }

            // Assert
            results.Should().BeEquivalentTo(delays);
        }

        [TestMethod]
        public void Calculate_InitialDelayOfZeroUsesDefault_Test()
        {
            // Arrange
            var sut = new ExponentialBackOffDelayStrategy(TimeSpan.FromHours(1), TimeSpan.Zero);
            var results = new List<int>();

            // Act
            for (var i = 1; i <= 6; i++)
            {
                results.Add((int)sut.Calculate(i).TotalSeconds);
            }

            // Assert
            results.Should().BeEquivalentTo(new[] { 100, 400, 900, 1600, 2500, 3600 });
        }
    }
}