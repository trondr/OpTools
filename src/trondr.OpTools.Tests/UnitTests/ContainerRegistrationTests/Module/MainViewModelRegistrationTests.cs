using NUnit.Framework;
using trondr.OpTools.Library.Module.ViewModels;
using trondr.OpTools.Tests.Common;

namespace trondr.OpTools.Tests.UnitTests.ContainerRegistrationTests.Module
{
    [TestFixture(Category=TestCategory.UnitTests)]
    public class MainViewModelRegistrationTests
    {
        [Test, RequiresSTA]
        public void MainViewModelRegistrationTest()
        {
            BootStrapperTestsHelper.CheckThatNumberOfResolvedServicesAre<MainViewModel>(1);
            BootStrapperTestsHelper.CheckThatResolvedServiceIsOfInstanceType<MainViewModel, MainViewModel>();
            BootStrapperTestsHelper.CheckThatResolvedServiceHasSingletonLifeCycle<MainViewModel>();
        }
    }
}