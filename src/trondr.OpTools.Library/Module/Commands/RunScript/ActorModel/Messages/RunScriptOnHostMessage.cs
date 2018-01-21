using LanguageExt;

namespace trondr.OpTools.Library.Module.Commands.RunScript.ActorModel.Messages
{
    public class RunScriptOnHostMessage: Record<RunScriptOnHostMessage>
    {
        public RunScriptOnHostMessage(Host host, string scriptPath, string resultPath)
        {
            Host = host;
            ScriptPath = scriptPath;
            ResultPath = resultPath;
        }

        public Host Host { get; }
        public string ScriptPath { get; }
        public string ResultPath { get; }
    }
}