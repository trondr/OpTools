using System.Net.NetworkInformation;
using Akka.Actor;

namespace trondr.OpTools.Library.Module.Commands.RunScript.ActorModel
{
    public class OnlineStatusActor: ReceiveActor
    {
        public OnlineStatusActor()
        {
            Receive<GetOnlineStatusMessage>(message => HandleGetOnLineStatusMessage(message));
            Receive<PingReply>(message => HandlePingReply(message));
        }

        private void HandleGetOnLineStatusMessage(GetOnlineStatusMessage message)
        {
            var senderClosure = Sender;
            var selfClosure = Self;
            var pingSender = new Ping();
            pingSender.SendPingAsync(message.IpAddress).PipeTo(selfClosure, senderClosure);
        }

        private void HandlePingReply(PingReply message)
        {
            Sender.Tell(new OnlineStatusMessage(message.Address, message.Status == IPStatus.Success));
        }
    }
}
