using LanguageExt;

namespace trondr.OpTools.Library.Module.Commands.RunScript.ActorModel.Messages
{
    public class RunScriptOnHostMessage: Record<RunScriptOnHostMessage>
    {
        public RunScriptOnHostMessage(Host host, string scriptPath, string resultFolderPath, bool resolveToIpv4Address)
        {
            Host = host;
            ScriptPath = scriptPath;
            ResultFolderPath = resultFolderPath;
            ResolveToIpv4Address = resolveToIpv4Address;
        }

        public Host Host { get; }
        public string ScriptPath { get; }
        public string ResultFolderPath { get; }
        public bool ResolveToIpv4Address { get; }
    }
}