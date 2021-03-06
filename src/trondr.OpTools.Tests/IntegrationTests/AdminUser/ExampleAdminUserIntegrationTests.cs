﻿using Common.Logging;
using Common.Logging.Simple;
using NUnit.Framework;
using trondr.OpTools.Tests.Common;

namespace trondr.OpTools.Tests.IntegrationTests.AdminUser
{
    [TestFixture(Category = TestCategory.AdminUserIntegrationTests)]
    public class ExampleAdminUserIntegrationTests
    {
        private ConsoleOutLogger _logger;

        [SetUp]
        public void SetUp()
        {
            _logger = new ConsoleOutLogger(this.GetType().Name, LogLevel.All, true, false, false, "yyyy-MM-dd hh:mm:ss");            
        }

        [TearDown]
        public void TearDown()
        {

        }

        [Test]
        public void ExampleTest1()
        {
            //var target = new ExampleServive(_logger);
            //var expected = "SomeExpectedValue";
            //var actual = target.GetSomeValue()
            //Assert.AreEqual(expected,actual);
        }        
    }
}