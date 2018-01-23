using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using trondr.OpTools.Library.Module;
using trondr.OpTools.Library.Module.Commands.RunScript;

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

        [Test]
        [TestCase("hostname", false)]
        [TestCase("1hostname", false)]
        [TestCase("127.1.0.2.2", false)]
        [TestCase("127.1.0.2", true)]
        public void IsValidIpAddressTest(string ipAddress, bool expected)
        {
            var actual = F.IsValidIpAddress(ipAddress);
            Assert.AreEqual(expected, actual, $"{ipAddress}");
        }

        [Test]        
        public void GetOnlineHostsToProcessTest()
        {
            var testHosts = GetTestHosts8of10IsOnline();           
            var actual = F.GetOnlineHostsToProcess(hosts: testHosts, samplePercent: 100).ToList();
            Assert.AreEqual(expected:8,actual:actual.Count,message: "Number of hosts");
        }

        [Test]
        public void GetOnlineHostsToProcessSample20Test()
        {
            var testHosts = GetTestHosts8of10IsOnline();
            var actual = F.GetOnlineHostsToProcess(hosts: testHosts, samplePercent: 20).ToList();
            Assert.AreEqual(expected: 1, actual: actual.Count, message: "Number of hosts");
        }

        [Test]
        public void GetOnlineHostsToProcessSample30Test()
        {
            var testHosts = GetTestHosts8of10IsOnline();
            var actual = F.GetOnlineHostsToProcess(hosts: testHosts, samplePercent: 30).ToList();
            Assert.AreEqual(expected: 2, actual: actual.Count, message: "Number of hosts");
        }

        [Test]
        public void GetOnlineHostsToProcessSample40Test()
        {
            var testHosts = GetTestHosts8of10IsOnline();
            var actual = F.GetOnlineHostsToProcess(hosts: testHosts, samplePercent: 40).ToList();
            Assert.AreEqual(expected: 3, actual: actual.Count, message: "Number of hosts");
        }

        [Test]
        public void GetOnlineHostsToProcessSample50Test()
        {
            var testHosts = GetTestHosts8of10IsOnline();
            var actual = F.GetOnlineHostsToProcess(hosts: testHosts, samplePercent: 50).ToList();
            Assert.AreEqual(expected: 4, actual: actual.Count, message: "Number of hosts");
        }

        [Test]
        public void GetOnlineHostsToProcessSample55Test()
        {
            var testHosts = GetTestHosts8of10IsOnline();
            var actual = F.GetOnlineHostsToProcess(hosts: testHosts, samplePercent: 55).ToList();
            Assert.AreEqual(expected: 4, actual: actual.Count, message: "Number of hosts");
        }

        [Test]
        public void GetOnlineHostsToProcessSample60Test()
        {
            var testHosts = GetTestHosts8of10IsOnline();
            var actual = F.GetOnlineHostsToProcess(hosts: testHosts, samplePercent: 60).ToList();
            Assert.AreEqual(expected: 4, actual: actual.Count, message: "Number of hosts");
        }

        [Test]
        public void GetOnlineHostsToProcessSample70Test()
        {
            var testHosts = GetTestHosts8of10IsOnline();
            var actual = F.GetOnlineHostsToProcess(hosts: testHosts, samplePercent: 70).ToList();
            Assert.AreEqual(expected: 5, actual: actual.Count, message: "Number of hosts");
        }

        [Test]
        public void GetOnlineHostsToProcessSample80Test()
        {
            var testHosts = GetTestHosts8of10IsOnline();
            var actual = F.GetOnlineHostsToProcess(hosts: testHosts, samplePercent: 80).ToList();
            Assert.AreEqual(expected: 6, actual: actual.Count, message: "Number of hosts");
        }

        [Test]
        public void GetOnlineHostsToProcessSample90Test()
        {
            var testHosts = GetTestHosts8of10IsOnline();
            var actual = F.GetOnlineHostsToProcess(hosts: testHosts, samplePercent: 90).ToList();
            Assert.AreEqual(expected: 7, actual: actual.Count, message: "Number of hosts");
        }

        [Test]
        public void GetOnlineHostsToProcessSample95Test()
        {
            var testHosts = GetTestHosts8of10IsOnline();
            var actual = F.GetOnlineHostsToProcess(hosts: testHosts, samplePercent: 95).ToList();
            Assert.AreEqual(expected: 7, actual: actual.Count, message: "Number of hosts");
        }

        [Test]
        public void GetOnlineHostsToProcessSample99Test()
        {
            var testHosts = GetTestHosts8of10IsOnline();
            var actual = F.GetOnlineHostsToProcess(hosts: testHosts, samplePercent: 99).ToList();
            Assert.AreEqual(expected: 7, actual: actual.Count, message: "Number of hosts");
        }

        private Dictionary<HostName, Host> GetTestHosts8of10IsOnline()
        {
            var hostName01 = GetHostName("computer01");
            var hostName02 = GetHostName("computer02");
            var hostName03 = GetHostName("computer03");
            var hostName04 = GetHostName("computer04");
            var hostName05 = GetHostName("computer05");
            var hostName06 = GetHostName("computer06");
            var hostName07 = GetHostName("computer07");
            var hostName08 = GetHostName("computer08");
            var hostName09 = GetHostName("computer09");
            var hostName10 = GetHostName("computer10");

            var ipAddress01 = GetIpAddress("192.168.1.101");
            var ipAddress02 = GetIpAddress("192.168.1.102");
            var ipAddress03 = GetIpAddress("192.168.1.103");
            var ipAddress04 = GetIpAddress("192.168.1.104");
            var ipAddress05 = GetIpAddress("192.168.1.105");
            var ipAddress06 = GetIpAddress("192.168.1.106");
            var ipAddress07 = GetIpAddress("192.168.1.107");
            var ipAddress08 = GetIpAddress("192.168.1.108");
            var ipAddress09 = GetIpAddress("192.168.1.109");
            var ipAddress10 = GetIpAddress("192.168.1.110");

            var testHosts = new Dictionary<HostName, Host>()
            {
                {hostName01, new Host(hostName01, ipAddress01, OnlineStatus.Online)},
                {hostName02, new Host(hostName02, ipAddress02, OnlineStatus.Offline)},
                {hostName03, new Host(hostName03, ipAddress03, OnlineStatus.Online)},
                {hostName04, new Host(hostName04, ipAddress04, OnlineStatus.Online)},
                {hostName05, new Host(hostName05, ipAddress05, OnlineStatus.Online)},
                {hostName06, new Host(hostName06, ipAddress06, OnlineStatus.Online)},
                {hostName07, new Host(hostName07, ipAddress07, OnlineStatus.Online)},
                {hostName08, new Host(hostName08, ipAddress08, OnlineStatus.Online)},
                {hostName09, new Host(hostName09, ipAddress09, OnlineStatus.Offline)},
                {hostName10, new Host(hostName10, ipAddress10, OnlineStatus.Online)}
            };

            return testHosts;
        }


        HostName GetHostName(string hostNameString)
        {
            HostName hostName = null;
            HostName.Create(hostNameString).IfSucc(hostname => hostName = hostname);
            return hostName;
        }

        IpAddress GetIpAddress(string ipAddressString)
        {
            IpAddress ipAddress = null;
            IpAddress.Create(ipAddressString).IfSucc(address => ipAddress = address);
            return ipAddress;
        }


        [Test]
        [TestCase("localhost", "127.0.0.1", false)]
        public void Hostname2IpAddressTest(string hostNameString, string ipaddressExpected, bool expecteddIsFaulted)
        {
            var hostName = GetHostName(hostNameString);
            var actual = F.HostName2IpAddress(hostName);
            Assert.IsFalse(actual.IsFaulted);
            actual.IfSucc(actualIpAddress =>
            {
                Assert.AreEqual(ipaddressExpected, actualIpAddress.Value);
            });
        }
    }
}