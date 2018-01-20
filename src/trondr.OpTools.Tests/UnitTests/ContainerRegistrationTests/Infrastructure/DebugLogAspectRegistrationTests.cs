using NUnit.Framework;
using trondr.OpTools.Infrastructure.ContainerExtensions;
using trondr.OpTools.Tests.Common;

namespace trondr.OpTools.Tests.UnitTests.ContainerRegistrationTests.Infrastructure
{
    [TestFixture(Category=TestCategory.UnitTests)]
    public class DebugLogAspectRegistrationTests
    {
        [Test, RequiresSTA]
        public void DebugLogAspectRegistrationTest()
        {
            BootStrapperTestsHelper.CheckThatNumberOfResolvedServicesAre<DebugLogAspect>(1);
            BootStrapperTestsHelper.CheckThatResolvedServiceIsOfInstanceType<DebugLogAspect, DebugLogAspect>();
            BootStrapperTestsHelper.CheckThatResolvedServiceHasSingletonLifeCycle<DebugLogAspect>();
        }
    }
}