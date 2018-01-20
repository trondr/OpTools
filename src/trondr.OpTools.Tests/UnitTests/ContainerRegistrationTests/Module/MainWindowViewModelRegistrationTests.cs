using NUnit.Framework;
using trondr.OpTools.Library.Module.ViewModels;
using trondr.OpTools.Tests.Common;

namespace trondr.OpTools.Tests.UnitTests.ContainerRegistrationTests.Module
{
    [TestFixture(Category = TestCategory.UnitTests)]
    public class MainWindowViewModelRegistrationTests
    {
        [Test, RequiresSTA]
        public void MainWindowViewModelRegistrationTest()
        {
            BootStrapperTestsHelper.CheckThatNumberOfResolvedServicesAre<MainWindowViewModel>(1);
            BootStrapperTestsHelper.CheckThatResolvedServiceIsOfInstanceType<MainWindowViewModel, MainWindowViewModel>();
            BootStrapperTestsHelper.CheckThatResolvedServiceHasSingletonLifeCycle<MainWindowViewModel>();
        }
    }
}