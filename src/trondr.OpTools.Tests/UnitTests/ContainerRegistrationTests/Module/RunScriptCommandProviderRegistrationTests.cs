using NUnit.Framework;
using trondr.OpTools.Library.Infrastructure;
using trondr.OpTools.Library.Module.Commands.RunScript;
using trondr.OpTools.Tests.Common;

namespace trondr.OpTools.Tests.UnitTests.ContainerRegistrationTests.Module
{
    [TestFixture(Category=TestCategory.UnitTests)]
    public class RunScriptCommandProviderRegistrationTests
    {        
        [Test, RequiresSTA]
        public static void RunScriptCommandProviderRegistrationTest()
        {
            BootStrapperTestsHelper.CheckThatNumberOfResolvedServicesAre<IRunScriptCommandProvider>(1);
            BootStrapperTestsHelper.CheckThatResolvedServiceIsOfInstanceTypeName<IRunScriptCommandProvider>("IRunScriptCommandProviderProxy");
            BootStrapperTestsHelper.CheckThatResolvedServiceHasSingletonLifeCycle<IRunScriptCommandProvider>();
        }
    }
}