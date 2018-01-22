namespace trondr.OpTools.Library.Module.Commands.RunScript.ActorModel.Messages
{
    public class RunScriptOnHostIsDoneMessage
    {
        public RunScriptOnHostIsDoneMessage(Host host)
        {
            Host = host;
        }

        public Host Host { get; }
    }
}