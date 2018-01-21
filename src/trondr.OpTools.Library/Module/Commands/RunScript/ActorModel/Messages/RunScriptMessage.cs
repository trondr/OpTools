using System;
using System.IO;
using LanguageExt;

namespace trondr.OpTools.Library.Module.Commands.RunScript.ActorModel.Messages
{
    public class RunScriptMessage
    {
        public string ScriptPath { get; }
        public string HostNameListCsv { get; }
        public string ResultFolderPath { get; }
        public int SamplePercent { get; }
        public bool ResolveToIpv4Address { get; }
        public int ScriptExecutionParallelism { get; }

        private RunScriptMessage(string scriptPath, string hostNameListCsv, string resultFolderPath, int samplePercent, bool resolveToIpv4Address, int scriptExecutionParallelism)
        {
            ScriptPath = scriptPath;
            HostNameListCsv = hostNameListCsv;
            ResultFolderPath = resultFolderPath;
            SamplePercent = samplePercent;
            ResolveToIpv4Address = resolveToIpv4Address;
            ScriptExecutionParallelism = scriptExecutionParallelism;            
        }

        public static Result<RunScriptMessage> Create(string scriptPath, string hostNameListCsv, string resultFolderPath, int samplePercent, bool resolveToIpv4Address, int scriptExecutionParallelism)
        {
            if(!File.Exists(scriptPath))
                return new Result<RunScriptMessage>(new FileNotFoundException($"Script path '{scriptPath}' not found."));
            if (!File.Exists(hostNameListCsv))
                return new Result<RunScriptMessage>(new FileNotFoundException($"Host name list file path '{hostNameListCsv}' not found."));
            if (!Directory.Exists(resultFolderPath))
                return new Result<RunScriptMessage>(new DirectoryNotFoundException($"Result folder path '{resultFolderPath}' not found."));
            if(samplePercent <=0 || samplePercent >100)
                return new Result<RunScriptMessage>(new ArgumentOutOfRangeException($"Sample percent '{samplePercent}' must be in the range [1..100]"));
            if (scriptExecutionParallelism <= 0 || scriptExecutionParallelism > 255)
                return new Result<RunScriptMessage>(new ArgumentOutOfRangeException($"Script execution parallelism '{scriptExecutionParallelism}' must be in the range [1..255]"));
            return new Result<RunScriptMessage>(new RunScriptMessage( scriptPath,  hostNameListCsv,  resultFolderPath,  samplePercent,  resolveToIpv4Address,  scriptExecutionParallelism));
        }
    }
}