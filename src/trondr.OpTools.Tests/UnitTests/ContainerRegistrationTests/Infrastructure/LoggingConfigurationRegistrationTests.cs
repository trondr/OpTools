using NUnit.Framework;
using trondr.OpTools.Library.Infrastructure;
using trondr.OpTools.Tests.Common;

namespace trondr.OpTools.Tests.UnitTests.ContainerRegistrationTests.Infrastructure
{
    [TestFixture(Category=TestCategory.UnitTests)]
    public class LoggingConfigurationRegistrationTests
    {
        [Test, RequiresSTA]
        public void LoggingConfigurationRegistrationTest()
        {
            BootStrapperTestsHelper.CheckThatNumberOfResolvedServicesAre<ILoggingConfiguration>(1);
            BootStrapperTestsHelper.CheckThatResolvedServiceIsOfInstanceType<ILoggingConfiguration, LoggingConfiguration>();
            BootStrapperTestsHelper.CheckThatResolvedServiceHasSingletonLifeCycle<ILoggingConfiguration>();
        }
    }
}