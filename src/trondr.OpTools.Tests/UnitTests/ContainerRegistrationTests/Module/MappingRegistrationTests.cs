using AutoMapper;
using NUnit.Framework;
using trondr.OpTools.Tests.Common;

namespace trondr.OpTools.Tests.UnitTests.ContainerRegistrationTests.Module
{
    [TestFixture(Category = TestCategory.UnitTests)]
    public class MappingRegistrationTests
    {
        [Test, RequiresSTA]
        public void MappingRegistrationTest()
        {
            BootStrapperTestsHelper.CheckThatNumberOfResolvedServicesAre<Profile>(1, "This number must be adjusted by the developer when new mappings are defined.");
        }
    }
}