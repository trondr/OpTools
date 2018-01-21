using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Akka.Actor;
using Akka.DI.Core;
using Common.Logging;
using LanguageExt;
using trondr.OpTools.Library.Infrastructure;
using trondr.OpTools.Library.Module.Commands.RunScript.ActorModel;

namespace trondr.OpTools.Library.Module.Commands.RunScript
{
    public class RunScriptCommandProvider : CommandProvider, IRunScriptCommandProvider
    {          
        private readonly ILog _logger;

        public RunScriptCommandProvider(ILog logger)
        {     
            _logger = logger;
        }
        
        public int RunScript(string scriptPath, string hostNameListCsv, string resultFolderPath, int samplePercent,
            bool resolveToIpv4Address, int scriptExecutionParallelism, ActorSystem runScriptActorSystem)
        {            
            var onlineStatusActor = runScriptActorSystem.ActorOf(runScriptActorSystem.DI().Props<OnlineStatusActor>());
            //Get host names
            var hostNameResults = GetHostNames(hostNameListCsv);            
            foreach (var hostNameResult in hostNameResults)
            {
                hostNameResult.IfSucc(hostname =>
                {
                    var ipHostEntry = Dns.GetHostEntry(hostname.Value);
                    var ipv4Address = ipHostEntry.AddressList.First(address => address.AddressFamily == AddressFamily.InterNetwork);
                    var ipAddressResult = IpAddress.Create(ipv4Address.ToString());
                    ipAddressResult.IfSucc(ipAddress =>
                    {
                        onlineStatusActor.Tell(new GetOnlineStatusMessage(ipAddress));
                        _logger.Info($"TODO: {hostname.Value}");
                    });
                    ipAddressResult.IfFail(exception => { _logger.Error(exception.Message); });
                });
                hostNameResult.IfFail(exception =>
                {
                    _logger.Error($"{exception.Message}");
                });
            }
            Thread.Sleep(10000);
            runScriptActorSystem.Terminate().Wait();
            //runScriptActorSystem.WhenTerminated.Wait();
            ToDo.Implement(ToDoPriority.Critical,"trondr","Implement run script coordinator actor");
            return 0;
        }

        private IEnumerable<Result<Hostname>> GetHostNames(string hostNameListCsv)
        {
            yield return Hostname.Create("localhost");
            yield return Hostname.Create("127.0.0.1");
            yield return Hostname.Create("127.0.0.1.2");
        }
    }
}