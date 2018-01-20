using NUnit.Framework;
using trondr.OpTools.Infrastructure.ContainerExtensions;
using trondr.OpTools.Tests.Common;

namespace trondr.OpTools.Tests.UnitTests.ContainerRegistrationTests.Infrastructure
{
    [TestFixture(Category=TestCategory.UnitTests)]
    public class InfoLogAspectRegistrationTests
    {
        [Test, RequiresSTA]
        public void InfoLogAspectRegistrationTest()
        {
            BootStrapperTestsHelper.CheckThatNumberOfResolvedServicesAre<InfoLogAspect>(1);
            BootStrapperTestsHelper.CheckThatResolvedServiceIsOfInstanceType<InfoLogAspect, InfoLogAspect>();
            BootStrapperTestsHelper.CheckThatResolvedServiceHasSingletonLifeCycle<InfoLogAspect>();
        }
    }
}