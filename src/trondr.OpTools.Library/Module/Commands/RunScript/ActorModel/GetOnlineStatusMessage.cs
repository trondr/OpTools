namespace trondr.OpTools.Library.Module.Commands.RunScript.ActorModel
{
    public class GetOnlineStatusMessage
    {
        public GetOnlineStatusMessage(IpAddress ipAddress)
        {
            IpAddress = ipAddress;
        }

        public IpAddress IpAddress { get; }
    }
}