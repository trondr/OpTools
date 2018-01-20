using NUnit.Framework;
using trondr.OpTools.Library.Infrastructure;
using trondr.OpTools.Library.Module.Commands.Example;
using trondr.OpTools.Tests.Common;

namespace trondr.OpTools.Tests.UnitTests.ContainerRegistrationTests.Module
{
    [TestFixture(Category=TestCategory.UnitTests)]
    public class ExampleCommandProviderRegistrationTests
    {        
        [Test, RequiresSTA]
        public static void ExampleCommandProviderRegistrationTest()
        {
            BootStrapperTestsHelper.CheckThatNumberOfResolvedServicesAre<IExampleCommandProvider>(1);
            BootStrapperTestsHelper.CheckThatResolvedServiceIsOfInstanceTypeName<IExampleCommandProvider>("IExampleCommandProviderProxy");
            BootStrapperTestsHelper.CheckThatResolvedServiceHasSingletonLifeCycle<IExampleCommandProvider>();
        }

    }
}