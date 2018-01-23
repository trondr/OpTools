namespace trondr.OpTools.Library.Module.Commands.RunScript.ActorModel.Messages
{
    public class GetOnlineStatusMessage
    {
        public GetOnlineStatusMessage(HostName hostName)
        {
            HostName = hostName;
        }

        public HostName HostName { get; }
    }
}