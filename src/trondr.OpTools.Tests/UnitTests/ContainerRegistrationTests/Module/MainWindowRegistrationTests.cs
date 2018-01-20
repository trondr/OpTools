using NUnit.Framework;
using trondr.OpTools.Infrastructure;
using trondr.OpTools.Library.Module.Views;
using trondr.OpTools.Tests.Common;

namespace trondr.OpTools.Tests.UnitTests.ContainerRegistrationTests.Module
{
    [TestFixture(Category=TestCategory.UnitTests)]
    public class MainWindowRegistrationTests
    {
        [Test, RequiresSTA]
        public void MainWindowRegistrationTest()
        {
            BootStrapperTestsHelper.CheckThatNumberOfResolvedServicesAre<MainWindow>(1);
            BootStrapperTestsHelper.CheckThatResolvedServiceIsOfInstanceType<MainWindow, MainWindow>();
            BootStrapperTestsHelper.CheckThatResolvedServiceHasSingletonLifeCycle<MainWindow>();
            using (var bootStrapper = new BootStrapper())
            {
                var target = bootStrapper.Container.ResolveAll<MainWindow>();
                Assert.IsNotNull(target[0].ViewModel, "View was null");                
            }
        }
    }
}