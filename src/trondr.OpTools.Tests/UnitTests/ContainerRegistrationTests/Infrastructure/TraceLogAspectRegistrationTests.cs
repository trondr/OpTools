using NUnit.Framework;
using trondr.OpTools.Infrastructure.ContainerExtensions;
using trondr.OpTools.Tests.Common;

namespace trondr.OpTools.Tests.UnitTests.ContainerRegistrationTests.Infrastructure
{
    [TestFixture(Category=TestCategory.UnitTests)]
    public class TraceLogAspectRegistrationTests
    {
        [Test, RequiresSTA]
        public void TraceLogAspectRegistrationTest()
        {
            BootStrapperTestsHelper.CheckThatNumberOfResolvedServicesAre<TraceLogAspect>(1);
            BootStrapperTestsHelper.CheckThatResolvedServiceIsOfInstanceType<TraceLogAspect, TraceLogAspect>();
            BootStrapperTestsHelper.CheckThatResolvedServiceHasSingletonLifeCycle<TraceLogAspect>();
        }
    }
}