using NUnit.Framework;
using trondr.OpTools.Infrastructure;
using trondr.OpTools.Library.Module.Views;
using trondr.OpTools.Tests.Common;

namespace trondr.OpTools.Tests.UnitTests.ContainerRegistrationTests.Module
{
    [TestFixture(Category=TestCategory.UnitTests)]
    public class MainViewRegistrationTests
    {
        [Test, RequiresSTA]
        public void MainViewRegistrationTest()
        {
            BootStrapperTestsHelper.CheckThatNumberOfResolvedServicesAre<MainView>(1);
            BootStrapperTestsHelper.CheckThatResolvedServiceIsOfInstanceType<MainView, MainView>();
            BootStrapperTestsHelper.CheckThatResolvedServiceHasSingletonLifeCycle<MainView>();            
        }
    }
}