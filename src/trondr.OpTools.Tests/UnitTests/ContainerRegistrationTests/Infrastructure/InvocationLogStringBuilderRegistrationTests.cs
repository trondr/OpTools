using NUnit.Framework;
using trondr.OpTools.Infrastructure.ContainerExtensions;
using trondr.OpTools.Tests.Common;

namespace trondr.OpTools.Tests.UnitTests.ContainerRegistrationTests.Infrastructure
{
    [TestFixture(Category=TestCategory.UnitTests)]
    public class InvocationLogStringBuilderRegistrationTests
    {
        [Test, RequiresSTA]
        public void InvocationLogStringBuilderRegistrationTest()
        {
            BootStrapperTestsHelper.CheckThatNumberOfResolvedServicesAre<IInvocationLogStringBuilder>(1);
            BootStrapperTestsHelper.CheckThatResolvedServiceIsOfInstanceType<IInvocationLogStringBuilder, InvocationLogStringBuilder>();
        }
    }
}