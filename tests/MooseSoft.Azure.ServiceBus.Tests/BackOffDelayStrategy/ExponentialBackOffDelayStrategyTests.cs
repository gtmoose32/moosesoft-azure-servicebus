using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moosesoft.Azure.ServiceBus.BackOffDelayStrategy;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Moosesoft.Azure.ServiceBus.Tests.BackOffDelayStrategy
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
    }
}