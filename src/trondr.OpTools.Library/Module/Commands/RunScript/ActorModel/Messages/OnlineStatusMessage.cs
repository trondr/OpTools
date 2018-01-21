using LanguageExt;

namespace trondr.OpTools.Library.Module.Commands.RunScript.ActorModel.Messages
{
    public class OnlineStatusMessage: Record<OnlineStatusMessage>
    {
        public Host Host { get; }
        
        public OnlineStatusMessage(Host host)
        {
            Host = host;
        }
    }
}