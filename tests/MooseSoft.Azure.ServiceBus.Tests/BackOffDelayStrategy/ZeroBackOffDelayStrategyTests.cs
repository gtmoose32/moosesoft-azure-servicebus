﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moosesoft.Azure.ServiceBus.BackOffDelayStrategy;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Moosesoft.Azure.ServiceBus.Tests.BackOffDelayStrategy
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ZeroBackOffDelayStrategyTests
    {
        [TestMethod]
        public void Calculate_Test()
        {
            //Arrange
            var expected = TimeSpan.Zero;
            var sut = new ZeroBackOffDelayStrategy();
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