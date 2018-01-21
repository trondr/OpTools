using System.Net;

namespace trondr.OpTools.Library.Module.Commands.RunScript.ActorModel
{
    public class OnlineStatusMessage
    {
        public IPAddress IpAddress { get; }
        public bool IsOnline { get; }

        public OnlineStatusMessage(IPAddress ipAddress, bool isOnline)
        {
            IpAddress = ipAddress;
            IsOnline = isOnline;
        }
    }
}