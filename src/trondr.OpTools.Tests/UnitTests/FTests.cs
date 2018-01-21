using NUnit.Framework;
using trondr.OpTools.Library.Module;

namespace trondr.OpTools.Tests.UnitTests
{
    [TestFixture()]
    public class FTests
    {
        [Test]
        [TestCase("hostname",true)]
        [TestCase("1hostname", false)]
        [TestCase("127.1.0.2.2", false)]
        [TestCase("127.1.0.2", true)]
        public void IsValidHostNameTest(string hostName, bool expected)
        {
            var actual = F.IsValidHostName(hostName);
            Assert.AreEqual(expected, actual, $"{hostName}");
        }
    }
}