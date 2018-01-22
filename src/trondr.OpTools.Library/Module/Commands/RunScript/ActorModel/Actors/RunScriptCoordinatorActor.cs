using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using Akka.DI.Core;
using Akka.Event;
using Akka.Routing;
using LanguageExt;
using trondr.OpTools.Library.Infrastructure;
using trondr.OpTools.Library.Module.Commands.RunScript.ActorModel.Messages;

namespace trondr.OpTools.Library.Module.Commands.RunScript.ActorModel.Actors
{
    public class RunScriptCoordinatorActor : ReceiveActor
    {
        private ILoggingAdapter Logger => _logger ?? (_logger = Context.GetLogger());

        private Dictionary<Hostname, Host> _hosts = new Dictionary<Hostname, Host>();        
        private ILoggingAdapter _logger;
        private int _numerOfOnlineStatusMessagesReceived;        
        private int _totalNumberOfHostsToCheck;
        private RunScriptMessage _runScriptMessage;
        private int _runScriptsOnHostsCount;
        private int _runScriptsOnHostsDoneCount;

        public RunScriptCoordinatorActor()
        {
            Receive<RunScriptMessage>(message => HandleRunScriptMessage(message));
            Receive<OnlineStatusMessage>(message => HandleOnlineStatusMessage(message));
            Receive<OnLineStatusCheckIsDoneMessage>(message => HandleOnLineStatusCheckIsDoneMessage());
            Receive<ProcessingIsDoneMessage>(message => HandleProcessingIsDoneMessage());
            Receive<RunScriptOnHostIsDoneMessage>(message => HandleRunScriptOnHostIsDoneMessage(message));
        }

        private void HandleRunScriptOnHostIsDoneMessage(RunScriptOnHostIsDoneMessage message)
        {
            Logger.Info($"Finished running script on {message.Host.HostName.Value}");
            _runScriptsOnHostsDoneCount++;
            if (_runScriptsOnHostsDoneCount >= _runScriptsOnHostsCount)
            {
                Logger.Info("Processing is done!");
                Self.Tell(new ProcessingIsDoneMessage());
            }
        }

        private void HandleProcessingIsDoneMessage()
        {
            Logger.Info("Terminating...");
            Context.System.Terminate();
        }

        private void HandleOnLineStatusCheckIsDoneMessage()
        {
            var onlineHostsToProcess = F.GetOnlineHostsToProcess(_hosts,_runScriptMessage.SamplePercent).ToList();
            var props = Context.DI().Props<RunScriptOnHostActor>().WithRouter(new SmallestMailboxPool(_runScriptMessage.ScriptExecutionParallelism));
            var runScriptOnHostActorPool = Context.ActorOf(props, "RunScriptOnHostActorPool");
            Logger.Info($"Running script '{_runScriptMessage.ScriptPath}' on {onlineHostsToProcess.Count} hosts");
            _runScriptsOnHostsCount = 0;
            foreach (var host in onlineHostsToProcess)
            {
                runScriptOnHostActorPool.Tell(new RunScriptOnHostMessage(host,_runScriptMessage.ScriptPath,_runScriptMessage.ResultFolderPath,_runScriptMessage.ResolveToIpv4Address));
                _runScriptsOnHostsCount++;
            }
        }
        private void SetState(RunScriptMessage runScriptMessage)
        {
            _numerOfOnlineStatusMessagesReceived = 0;            
            _totalNumberOfHostsToCheck = 0;
            _runScriptsOnHostsCount = 0;
            _runScriptsOnHostsDoneCount = 0;
        _hosts.Clear();
            _runScriptMessage = runScriptMessage;
        }

        private void HandleOnlineStatusMessage(OnlineStatusMessage message)
        {
            _numerOfOnlineStatusMessagesReceived++;
            Logger.Info($"{message.Host}");
            if (!_hosts.ContainsKey(message.Host.HostName))
            {
                _hosts.Add(message.Host.HostName, message.Host);
            }
            else
            {
                Logger.Warning($"Hostname '{message.Host.HostName}' has allready been checked for online status.");
            }

            if (_numerOfOnlineStatusMessagesReceived >= _totalNumberOfHostsToCheck)
            {
                Self.Tell(new OnLineStatusCheckIsDoneMessage());
            }
        }

        private void HandleRunScriptMessage(RunScriptMessage runScriptMessage)
        {
            Logger.Info("Preparing to run script agains list of hosts.");
            SetState(runScriptMessage);
            var props = Context.DI().Props<OnlineStatusActor>().WithRouter(new SmallestMailboxPool(runScriptMessage.ScriptExecutionParallelism));
            var onlineStatusActorPool = Context.ActorOf(props, "OnlineStatusActorPool");

            Logger.Info("Get host names...");
            var hostNameResults = GetHostNames(runScriptMessage.HostNameListCsv).ToList();

            _totalNumberOfHostsToCheck = hostNameResults.Count;
            Logger.Info($"Checking online status of {_totalNumberOfHostsToCheck} host names...");
            foreach (var hostNameResult in hostNameResults)
            {
                hostNameResult.IfSucc(hostname =>
                {
                    var ipAddressResult = F.HostName2IpAddress(hostname);
                    ipAddressResult.IfSucc(ipAddress =>
                    {
                        var localHostName = hostname;
                        var localIpAddress = ipAddress;
                        onlineStatusActorPool.Tell(new GetOnlineStatusMessage(new Host(localHostName, localIpAddress, OnlineStatus.Unknown)));
                    });
                    ipAddressResult.IfFail(exception =>
                    {
                        Logger.Error(exception.Message);
                        _totalNumberOfHostsToCheck--;
                    });
                });
                hostNameResult.IfFail(exception =>
                {
                    Logger.Error($"{exception.Message}");
                    _totalNumberOfHostsToCheck--;
                });
            }
            Logger.Info($"Ended up checking online status of {_totalNumberOfHostsToCheck} host names.");
        }

        private IEnumerable<Result<Hostname>> GetHostNames(string hostNameListCsv)
        {
            ToDo.Implement(ToDoPriority.Critical,"trondr","Implement Csv parsing from file");
            yield return Hostname.Create("localhost");
            yield return Hostname.Create("127.0.0.1");
            yield return Hostname.Create("localhost");
            yield return Hostname.Create("127.0.0.1");
            yield return Hostname.Create("adrdc01trix");
            yield return Hostname.Create("127.0.0.1.2");
        }
    }
}
