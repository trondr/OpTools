using Castle.Facilities.TypedFactory;
using NUnit.Framework;
using trondr.OpTools.Infrastructure;
using trondr.OpTools.Infrastructure.ContainerExtensions;
using trondr.OpTools.Tests.Common;

namespace trondr.OpTools.Tests.UnitTests.ContainerRegistrationTests.Infrastructure
{
    [TestFixture(Category=TestCategory.UnitTests)]
    public class TypedFactoryComponentSelectorRegistrationTests
    {
        [Test, RequiresSTA]
        public void TypedFactoryComponentSelectorRegistrationTest()
        {
            BootStrapperTestsHelper.CheckThatNumberOfResolvedServicesAre<ITypedFactoryComponentSelector>(3);
            BootStrapperTestsHelper.CheckThatResolvedServiceHasSingletonLifeCycle<ITypedFactoryComponentSelector>();
            using (var bootStrapper = new BootStrapper())
            {
                var target = bootStrapper.Container.ResolveAll<ITypedFactoryComponentSelector>();
                Assert.AreEqual(typeof(CustomTypeFactoryComponentSelector), target[2].GetType(), "The third ITypedFactoryComponentSelector instance was not of type CustomTypeFactoryComponentSelector");
            }
        }
    }
}