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
    public class LinearBackOffDelayStrategyTests
    {
        [TestMethod]
        public void Calculate_Test()
        {
            //Arrange
            var sut = LinearBackOffDelayStrategy.Default;
            var results = new List<TimeSpan>();

            //Act
            for (var i = 1; i <= 10; i++)
            {
                results.Add(sut.Calculate(i));
            }
            
            //Assert
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            Assert.IsTrue(results.All(ts => (ts.TotalMinutes % 2) == 0 ));

            var previous = TimeSpan.Zero;
            foreach (var ts in results)
            {
                Assert.AreEqual(previous + TimeSpan.FromMinutes(2), ts);
                previous = ts;
            }
        }
    }
}