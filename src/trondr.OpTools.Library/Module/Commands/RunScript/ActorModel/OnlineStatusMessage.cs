namespace trondr.OpTools.Library.Module.Commands.RunScript.ActorModel
{
    public class OnlineStatusMessage
    {
        public IpAddress IpAddress { get; }
        public bool IsOnline { get; }

        public OnlineStatusMessage(IpAddress ipAddress, bool isOnline)
        {
            IpAddress = ipAddress;
            IsOnline = isOnline;
        }
    }
}