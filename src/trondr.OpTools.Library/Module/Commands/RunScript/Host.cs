using LanguageExt;

namespace trondr.OpTools.Library.Module.Commands.RunScript
{
    public class Host: Record<Host>
    {
        public HostName HostName { get; }
        [OptOutOfHashCode]
        public IpAddress IpAddress { get; }
        [OptOutOfHashCode]
        public OnlineStatus OnlineStatus { get;}

        public Host(HostName hostName, IpAddress ipAddress, OnlineStatus onlineStatus)
        {
            HostName = hostName;
            IpAddress = ipAddress;
            OnlineStatus = onlineStatus;
        }
    }
}
