using Akka.Actor;
using Akka.DI.CastleWindsor;
using Akka.DI.Core;
using Castle.Windsor;
using NCmdLiner.Attributes;
using trondr.OpTools.Library.Infrastructure;
using trondr.OpTools.Library.Module.Commands.RunScript;

namespace trondr.OpTools.Module.Commands
{
    public class RunScriptCommandDefinition : CommandDefinition
    {
        private readonly IRunScriptCommandProvider _runScriptCommandProvider;
        private readonly IWindsorContainer _windsorContainer;

        public RunScriptCommandDefinition(IRunScriptCommandProvider runScriptCommandProvider, IWindsorContainer windsorContainer)
        {
            _runScriptCommandProvider = runScriptCommandProvider;
            _windsorContainer = windsorContainer;
        }

        [Command(Description = "Run PowerShell script against all (or a random sample of) targets in a host list. The online status (ping) of each host will be checked before attempting to run the script. The script will run concurrently to speed up the overall processing time. The Powershell script should supports two input parameters: The computer name or ip address of the target host 'HostName' and the result path 'ResultFolderPath'. The powershell script is itself responsible for accessing the remote host and uploading the result to the given result path.", Summary = "Run PowerShell script against all (or a random sample of) targets in a host list.")]
        public int RunScript(
            [RequiredCommandParameter(Description = "Path to the Powershell script.", AlternativeName = "sp", ExampleValue = @"c:\temp\test.ps1")]
            string scriptPath,
            [RequiredCommandParameter(Description = "Path to a csv file with list of host names to run the Powershell script against. Csv format: HostName.", AlternativeName = "hnl", ExampleValue = @"c:\temp\hostnames.csv")]
            string hostNameListCsv,
            [RequiredCommandParameter(Description = "Result folder path. Each script execution can upload the result to this path.", AlternativeName = "rfp", ExampleValue = @"c:\temp")]
            string resultFolderPath,
            [OptionalCommandParameter(Description = "Specify a samplePercent less than 100 to run the script against a random sample of the host names in the host list. A value of 100 (default) means the script will run against all host names in the list.", AlternativeName = "sap", DefaultValue = 100, ExampleValue = 100)]
            int samplePercent,
            [OptionalCommandParameter(Description = "Resolve host name to ip v4 address before executing script.",AlternativeName = "rip",DefaultValue = false, ExampleValue = false)]
            bool resolveToIpv4Address,
            [OptionalCommandParameter(Description = "The number of concurrent script executions.", AlternativeName = "sep", DefaultValue = 10, ExampleValue = 10)]
            int scriptExecutionParallelism
            )
        {
            using (var runScriptActorSystem = ActorSystem.Create("RunScriptActorSystem"))
            {
                var windsorDependencyResolver = new WindsorDependencyResolver(_windsorContainer, runScriptActorSystem);
                runScriptActorSystem.AddDependencyResolver(windsorDependencyResolver);
                var exitCode = _runScriptCommandProvider.RunScript(scriptPath, hostNameListCsv, resultFolderPath, samplePercent, resolveToIpv4Address, scriptExecutionParallelism, runScriptActorSystem);
                return exitCode;
            }
        }
    }
}
