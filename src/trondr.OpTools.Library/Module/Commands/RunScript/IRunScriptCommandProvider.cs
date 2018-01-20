namespace trondr.OpTools.Library.Module.Commands.RunScript
{
    public interface IRunScriptCommandProvider
    {        
        int RunScript(string scriptPath, string hostNameListCsv, string resultFolderPath, int samplePercent, bool resolveToIpv4Address, int scriptExecutionParallelism);
    }
}
