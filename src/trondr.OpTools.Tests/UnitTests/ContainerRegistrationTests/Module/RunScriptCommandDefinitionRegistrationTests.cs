using NUnit.Framework;
using trondr.OpTools.Module.Commands;
using trondr.OpTools.Library.Infrastructure;
using trondr.OpTools.Tests.Common;

namespace trondr.OpTools.Tests.UnitTests.ContainerRegistrationTests.Module
{
    [TestFixture(Category=TestCategory.UnitTests)]
    public class RunScriptCommandDefinitionRegistrationTests
    {        
        [Test, RequiresSTA]
        public static void RunScriptCommandDefinitionRegistrationTest()
        {
            BootStrapperTestsHelper.CheckThatOneOfTheResolvedServicesAre<CommandDefinition, RunScriptCommandDefinition>("Not registered: " + typeof(RunScriptCommandDefinition).FullName);
        }        
    }
}