using NUnit.Framework;
using trondr.OpTools.Module.Commands;
using trondr.OpTools.Library.Infrastructure;
using trondr.OpTools.Tests.Common;

namespace trondr.OpTools.Tests.UnitTests.ContainerRegistrationTests.Module
{
    [TestFixture(Category=TestCategory.UnitTests)]
    public class ExampleCommandDefinitionRegistrationTests
    {        
        [Test, RequiresSTA]
        public static void ExampleCommandDefinitionRegistrationTest()
        {
            BootStrapperTestsHelper.CheckThatOneOfTheResolvedServicesAre<CommandDefinition, ExampleCommandDefinition>("Not registered: " + typeof(ExampleCommandDefinition).FullName);
        }        
    }
}