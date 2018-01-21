using LanguageExt;

namespace trondr.OpTools.Library.Module.Commands.RunScript
{
    public class Host: Record<Host>
    {
        public Hostname HostName { get; }
        [OptOutOfHashCode]
        public IpAddress IpAddress { get; }
        [OptOutOfHashCode]
        public OnlineStatus OnlineStatus { get;}

        public Host(Hostname hostName, IpAddress ipAddress, OnlineStatus onlineStatus)
        {
            HostName = hostName;
            IpAddress = ipAddress;
            OnlineStatus = onlineStatus;
        }
    }
}
