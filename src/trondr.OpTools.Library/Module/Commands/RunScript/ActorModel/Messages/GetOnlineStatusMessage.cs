namespace trondr.OpTools.Library.Module.Commands.RunScript.ActorModel.Messages
{
    public class GetOnlineStatusMessage
    {
        public GetOnlineStatusMessage(Host host)
        {
            Host = host;
        }

        public Host Host { get; }
    }
}