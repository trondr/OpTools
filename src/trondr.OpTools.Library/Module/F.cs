using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using LanguageExt;
using trondr.OpTools.Library.Module.Commands.RunScript;

namespace trondr.OpTools.Library.Module
{
    public static class F
    {
        private static readonly Regex ValidIpAddressRegex = new Regex(@"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$");
        private static readonly Regex ValidHostnameRegex = new Regex(@"^(([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*([A-Za-z0-9]|[A-Za-z0-9][A-Za-z0-9\-]*[A-Za-z0-9])$");
        private static readonly Regex Valid952HostnameRegex = new Regex(@"^(([a-zA-Z]|[a-zA-Z][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*([A-Za-z]|[A-Za-z][A-Za-z0-9\-]*[A-Za-z0-9])$");

        public static bool IsValidHostName(string hostName)
        {
            return Valid952HostnameRegex.IsMatch(hostName) || ValidIpAddressRegex.IsMatch(hostName);
        }

        public static bool IsValidIpAddress(string ipAdress)
        {
            return ValidIpAddressRegex.IsMatch(ipAdress);
        }

        public static IEnumerable<Host> GetOnlineHostsToProcess(Dictionary<HostName, Host> hosts, int samplePercent)
        {
            var onlineHosts = hosts
                .Where(pair => pair.Value.OnlineStatus == OnlineStatus.Online)
                .Select(pair => pair.Value);
            if(samplePercent == 100)
                return onlineHosts;
            return SampleHosts(onlineHosts, samplePercent);
        }

        private static IEnumerable<Host> SampleHosts(IEnumerable<Host> onlineHosts, int samplePercent)
        {
            var onLineHostsArray = onlineHosts.ToArray();
            var hostsToReturnCount = onLineHostsArray.Length * samplePercent / 100;
            var random = new Random();
            var returnedHostsCount = 0;
            var allreadyReturned = new Dictionary<int,int>();
            while (returnedHostsCount < hostsToReturnCount)
            {
                var indexToReturn = random.Next(0, onLineHostsArray.Length);
                if(!allreadyReturned.ContainsKey(indexToReturn))
                {
                    allreadyReturned.Add(indexToReturn,indexToReturn);
                    yield return onLineHostsArray[indexToReturn];
                    returnedHostsCount++;
                }
            }            
        }

        public static Result<IpAddress> HostName2IpAddress(HostName hostName)
        {
            try
            {
                var ipHostEntry = Dns.GetHostEntry(hostName.Value);
                var ipv4Address = ipHostEntry.AddressList.First(address => address.AddressFamily == AddressFamily.InterNetwork);
                var ipAddressResult = IpAddress.Create(ipv4Address.ToString());
                return ipAddressResult;
            }
            catch (Exception ex)
            {
                return new Result<IpAddress>(new ArgumentException($"Failed to look up '{hostName.Value}' in the DNS. {ex.Message}"));
            }            
        }        
    }
}
