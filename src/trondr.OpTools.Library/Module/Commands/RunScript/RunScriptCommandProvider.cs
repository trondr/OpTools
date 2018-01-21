using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Akka.Dispatch.SysMsg;
using Common.Logging;
using LanguageExt;
using LanguageExt;
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
            //Get host names
            var hostNameResults = GetHostNames(hostNameListCsv);            
            foreach (var hostNameResult in hostNameResults)
            {
                hostNameResult.IfSucc(hostname =>
                {
                    _logger.Info($"TODO: {hostname.Value}");
                });
                hostNameResult.IfFail(exception =>
                {
                    _logger.Error($"{exception.Message}");
                });
            }
            throw new System.NotImplementedException();
        }

        private IEnumerable<Result<Hostname>> GetHostNames(string hostNameListCsv)
        {
            yield return Hostname.Create("localhost");
            yield return Hostname.Create("127.0.0.1");
            yield return Hostname.Create("127.0.0.1.2");
        }
    }
}