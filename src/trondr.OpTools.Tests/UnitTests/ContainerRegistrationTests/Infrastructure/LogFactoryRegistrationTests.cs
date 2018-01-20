using NUnit.Framework;
using trondr.OpTools.Infrastructure.ContainerExtensions;
using trondr.OpTools.Tests.Common;

namespace trondr.OpTools.Tests.UnitTests.ContainerRegistrationTests.Infrastructure
{
    [TestFixture(Category=TestCategory.UnitTests)]
    public class LogFactoryRegistrationTests
    {
        [Test, RequiresSTA]
        public void LogFactoryRegistrationTest()
        {
            BootStrapperTestsHelper.CheckThatNumberOfResolvedServicesAre<ILogFactory>(1);
            BootStrapperTestsHelper.CheckThatResolvedServiceIsOfInstanceType<ILogFactory, LogFactory>();
            BootStrapperTestsHelper.CheckThatResolvedServiceHasSingletonLifeCycle<ILogFactory>();
        }
    }
}