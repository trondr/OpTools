using NCmdLiner;
using NUnit.Framework;
using trondr.OpTools.Infrastructure;
using trondr.OpTools.Tests.Common;

namespace trondr.OpTools.Tests.UnitTests.ContainerRegistrationTests.Infrastructure
{
    [TestFixture(Category=TestCategory.UnitTests)]
    public class NCmdLinerIMessengerRegistrationTests
    {
        [Test, RequiresSTA]
        public void NCmdLinerIMessengerRegistrationTest()
        {
            BootStrapperTestsHelper.CheckThatNumberOfResolvedServicesAre<IMessenger>(1);
            BootStrapperTestsHelper.CheckThatResolvedServiceIsOfInstanceType<IMessenger, NotepadMessenger>();
            BootStrapperTestsHelper.CheckThatResolvedServiceHasSingletonLifeCycle<IMessenger>();
        }
    }
}