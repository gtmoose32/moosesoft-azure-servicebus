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
    public class ConstantBackOffDelayStrategyTests
    {
        [TestMethod]
        public void Calculate_Test()
        {
            //Arrange
            var expected = TimeSpan.FromMinutes(5);
            var sut = ConstantBackOffDelayStrategy.Default;
            var results = new List<TimeSpan>();

            //Act
            for (var i = 1; i <= 10; i++)
            {
                results.Add(sut.Calculate(i));
            }
            
            //Assert
            Assert.IsTrue(results.All(ts => ts == expected));

        }
    }
}