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
            var pingSender = new Ping();
            if (Logger.IsDebugEnabled) Logger.Debug($"Pinging {message.Host.IpAddress.Value} from {Context.Self.Path}");
            var pingReply = pingSender.Send(message.Host.IpAddress.Value);
            var host = new Host(message.Host.HostName, message.Host.IpAddress, PingReply2OnlineStatus(pingReply));
            var onlineStatusMessage = new OnlineStatusMessage(host);
            if(Logger.IsDebugEnabled) Logger.Debug($"Sending on line status message result '{onlineStatusMessage}' back to sender {Context.Sender.Path}");
            Sender.Tell(onlineStatusMessage);
        }

        private OnlineStatus PingReply2OnlineStatus(PingReply pingReply)
        {
            if (pingReply.Status == IPStatus.Success)
                return OnlineStatus.Online;
            return OnlineStatus.Offline;
        }
    }
}
