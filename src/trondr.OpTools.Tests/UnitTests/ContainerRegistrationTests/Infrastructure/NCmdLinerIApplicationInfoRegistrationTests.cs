using NCmdLiner;
using NUnit.Framework;
using trondr.OpTools.Tests.Common;

namespace trondr.OpTools.Tests.UnitTests.ContainerRegistrationTests.Infrastructure
{
    [TestFixture(Category=TestCategory.UnitTests)]
    public class NCmdLinerIApplicationInfoRegistrationTests
    {
        [Test, RequiresSTA]
        public void NCmdLinerIApplicationInfoRegistrationTest()
        {
            BootStrapperTestsHelper.CheckThatNumberOfResolvedServicesAre<IApplicationInfo>(1);
            BootStrapperTestsHelper.CheckThatResolvedServiceIsOfInstanceType<IApplicationInfo, ApplicationInfo>();
            BootStrapperTestsHelper.CheckThatResolvedServiceHasSingletonLifeCycle<IApplicationInfo>();
        }
    }
}