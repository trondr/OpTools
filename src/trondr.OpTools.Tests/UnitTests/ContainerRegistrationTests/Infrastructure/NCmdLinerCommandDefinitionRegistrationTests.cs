using NUnit.Framework;
using trondr.OpTools.Library.Infrastructure;
using trondr.OpTools.Tests.Common;

namespace trondr.OpTools.Tests.UnitTests.ContainerRegistrationTests.Infrastructure
{
    [TestFixture(Category=TestCategory.UnitTests)]
    public class NCmdLinerCommandDefinitionRegistrationTests
    {
        [Test, RequiresSTA]
        public void NCmdLinerCommandDefinitionRegistrationTest()
        {
            BootStrapperTestsHelper.CheckThatNumberOfResolvedServicesAre<CommandDefinition>(1, "The number should equal the number of service implementations. The programmer should manually adjust the expected number in this unit test for each added or removed service implementation.");
        }
    }
}