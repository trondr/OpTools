using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Akka.Actor;
using Akka.DI.Core;
using Akka.Event;
using Akka.Routing;
using LanguageExt;
using trondr.OpTools.Library.Module.Commands.RunScript.ActorModel.Messages;

namespace trondr.OpTools.Library.Module.Commands.RunScript.ActorModel.Actors
{
    public class RunScriptCoordinatorActor : ReceiveActor
    {
        private ILoggingAdapter Logger => _logger ?? (_logger = Context.GetLogger());

        private Dictionary<HostName, Host> _hosts = new Dictionary<HostName, Host>();        
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
            if (onlineHostsToProcess.Count == 0)
            {
                Logger.Info("Nothing to do.");
                Self.Tell(new ProcessingIsDoneMessage());
                return;
            }
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
            
            var hostNamesResult = GetHostNames(runScriptMessage.HostNameListCsv);
            hostNamesResult.IfSucc(hostNames =>
            {
                _totalNumberOfHostsToCheck = hostNames.Count;
                if (_totalNumberOfHostsToCheck == 0)
                {
                    Logger.Info("Nothing to do.");
                    Self.Tell(new ProcessingIsDoneMessage());
                    return;
                }
                Logger.Info($"Checking online status of {_totalNumberOfHostsToCheck} host names...");
                foreach (var hostName in hostNames)
                {
                    onlineStatusActorPool.Tell(new GetOnlineStatusMessage(hostName));                    
                }
            });
            hostNamesResult.IfFail(exception =>
            {
                Logger.Error($"Failed to parse csv file '{runScriptMessage.HostNameListCsv}'. {exception.Message}");
                Self.Tell(new ProcessingIsDoneMessage());
            });            
        }

        private Result<List<HostName>> GetHostNames(string hostNameListCsv)
        {
            Logger.Info($"Loading host names from csv file '{hostNameListCsv}'...");

            var hostNames = new List<HostName>();
            
            if(!File.Exists(hostNameListCsv))
                return new Result<List<HostName>>(new FileNotFoundException("Host name list csv file not found",hostNameListCsv));

            var fullFileName = Path.GetFullPath(hostNameListCsv);
            var fileLines = File.ReadAllLines(fullFileName);

            if(fileLines.Length <= 1)
                return new Result<List<HostName>>(new InvalidDataException("Host name list csv file is empty"));

            if(fileLines[0] != "HostName")
                return new Result<List<HostName>>(new FormatException("The one and only column name in the host name list csv file must be 'HostName'."));
            
            for (var i = 1; i < fileLines.Length; i++)
            {
                var hostNameRaw = fileLines[i].Trim();
                var hostNameResult =  HostName.Create(hostNameRaw);
                hostNameResult.IfSucc(hostName =>
                {
                    hostNames.Add(hostName);
                });
                hostNameResult.IfFail(exception =>
                {
                    Logger.Error($"Invalid host name. {exception.Message}");                    
                });
            }
            Logger.Info($"Loaded {hostNames.Count} host names from csv file.");
            return hostNames;
        }
    }
}
