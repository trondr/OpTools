using NUnit.Framework;
using trondr.OpTools.Library.Infrastructure;
using trondr.OpTools.Tests.Common;

namespace trondr.OpTools.Tests.UnitTests.ContainerRegistrationTests.Infrastructure
{
    [TestFixture(Category=TestCategory.UnitTests)]
    public class TypeMapperRegistrationTests
    {
        [Test, RequiresSTA]
        public void TypeMapperRegistrationTest()
        {
            BootStrapperTestsHelper.CheckThatNumberOfResolvedServicesAre<ITypeMapper>(1);
            BootStrapperTestsHelper.CheckThatResolvedServiceIsOfInstanceType<ITypeMapper, TypeMapper>();
            BootStrapperTestsHelper.CheckThatResolvedServiceHasSingletonLifeCycle<ITypeMapper>();
        }
    }
}