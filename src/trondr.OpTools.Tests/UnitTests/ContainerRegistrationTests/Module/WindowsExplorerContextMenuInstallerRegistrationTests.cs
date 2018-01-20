using NUnit.Framework;
using trondr.OpTools.Library.Module.Common.Install;
using trondr.OpTools.Tests.Common;

namespace trondr.OpTools.Tests.UnitTests.ContainerRegistrationTests.Module
{
    [TestFixture(Category=TestCategory.UnitTests)]
    public class WindowsExplorerContextMenuInstallerRegistrationTests
    {        
        [Test, RequiresSTA]
        public static void WindowsExplorerContextMenuInstallerRegistrationTest()
        {
            BootStrapperTestsHelper.CheckThatNumberOfResolvedServicesAre<IWindowsExplorerContextMenuInstaller>(1);
            BootStrapperTestsHelper.CheckThatResolvedServiceIsOfInstanceType<IWindowsExplorerContextMenuInstaller, WindowsExplorerContextMenuInstaller>();
            BootStrapperTestsHelper.CheckThatResolvedServiceHasSingletonLifeCycle<IWindowsExplorerContextMenuInstaller>();
        }

    }
}