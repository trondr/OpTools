using System.Net.NetworkInformation;
using Akka.Actor;
using Akka.Event;
using trondr.OpTools.Library.Module.Commands.RunScript.ActorModel.Messages;

namespace trondr.OpTools.Library.Module.Commands.RunScript.ActorModel.Actors
{
    public class OnlineStatusActor: ReceiveActor
    {
        private ILoggingAdapter _logger;
        private ILoggingAdapter Logger => _logger ?? (_logger = Context.GetLogger());

        public OnlineStatusActor()
        {            
            Receive<GetOnlineStatusMessage>(message => HandleGetOnLineStatusMessage(message));
        }

        private void HandleGetOnLineStatusMessage(GetOnlineStatusMessage message)
        {
            var ipAddressResult = F.HostName2IpAddress(message.HostName);
            ipAddressResult.IfSucc(ipAddress =>
            {
                var onlineStatusMessage = PingHost(message.HostName, ipAddress);
                if (Logger.IsDebugEnabled) Logger.Debug($"Sending on line status message result '{onlineStatusMessage}' back to sender {Context.Sender.Path}");
                Sender.Tell(onlineStatusMessage);                
            });
            ipAddressResult.IfFail(exception =>
            {
                Logger.Error(exception.Message);
                var host = new Host(message.HostName, IpAddress.Empty, OnlineStatus.UnknownHost);
                var onlineStatusMessage = new OnlineStatusMessage(host);
                Sender.Tell(onlineStatusMessage);                
            });
        }

        private OnlineStatusMessage PingHost(HostName hostName,IpAddress ipAddress)
        {
            PingReply pingReply;
            using (var pingSender = new Ping())
            {
                if (Logger.IsDebugEnabled) Logger.Debug($"Pinging {ipAddress.Value} from {Context.Self.Path}");
                pingReply = pingSender.Send(ipAddress.Value);
            }
            var host = new Host(hostName, ipAddress, PingReply2OnlineStatus(pingReply));
            var onlineStatusMessage = new OnlineStatusMessage(host);
            return onlineStatusMessage;
        }


        private OnlineStatus PingReply2OnlineStatus(PingReply pingReply)
        {
            if (pingReply.Status == IPStatus.Success)
                return OnlineStatus.Online;
            return OnlineStatus.Offline;
        }
    }
}
