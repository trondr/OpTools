using Akka.Actor;
using Akka.DI.Core;
using Common.Logging;
using trondr.OpTools.Library.Infrastructure;
using trondr.OpTools.Library.Module.Commands.RunScript.ActorModel;
using trondr.OpTools.Library.Module.Commands.RunScript.ActorModel.Actors;
using trondr.OpTools.Library.Module.Commands.RunScript.ActorModel.Messages;

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
            var exitCode = 0;

            _logger.Info("Starting run script coordinator");
            var runScriptCoordinatorActor = runScriptActorSystem.ActorOf(runScriptActorSystem.DI().Props<RunScriptCoordinatorActor>(), "RunScriptCoordinatorActor");

            _logger.Info("Parsing input parameters...");
            var runScriptMessageResult = RunScriptMessage.Create(scriptPath, hostNameListCsv, resultFolderPath, samplePercent, resolveToIpv4Address,
                scriptExecutionParallelism);

            runScriptMessageResult.IfSucc(runScriptMessage =>
            {
                _logger.Info($"Tell run script coordinator to start processing. Message: {runScriptMessage}...");
                runScriptCoordinatorActor.Tell(runScriptMessage);
            });
            
            runScriptMessageResult.IfFail(exception =>
            {
                _logger.Error($"Failed to parse input parameters. {exception.Message}");
                runScriptActorSystem.Terminate();
                exitCode = 1;
            });

            _logger.Info("Waiting for run script processing to finish");
            runScriptActorSystem.WhenTerminated.Wait();            
            return exitCode;
        }

        
    }
}