using Common.Logging;
using trondr.OpTools.Library.Infrastructure;

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
            bool resolveToIpv4Address, int scriptExecutionParallelism)
        {
            throw new System.NotImplementedException();
        }
    }
}