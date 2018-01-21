using System.Net.NetworkInformation;
using Akka.Actor;
using Akka.Event;
using Common.Logging;

namespace trondr.OpTools.Library.Module.Commands.RunScript.ActorModel
{
    public class OnlineStatusActor: ReceiveActor
    {
        //private readonly ILog _logger;
        private readonly ILoggingAdapter _logger = Context.GetLogger();

        public OnlineStatusActor()
        {
            //_logger = logger;
            Receive<GetOnlineStatusMessage>(message => HandleGetOnLineStatusMessage(message));
            Receive<PingReply>(message => HandlePingReply(message));
        }

        private void HandleGetOnLineStatusMessage(GetOnlineStatusMessage message)
        {
            var senderClosure = Sender;
            var selfClosure = Self;
            var pingSender = new Ping();
            _logger.Info($"Pinging {message.IpAddress.Value}");
            pingSender.SendPingAsync(message.IpAddress.Value).PipeTo(selfClosure, senderClosure);
        }

        private void HandlePingReply(PingReply message)
        {
            var ipAddressResult = IpAddress.Create(message.Address.ToString());
            ipAddressResult.IfSucc(ipAddress =>
            {
                var onlineStatusMessage = new OnlineStatusMessage(ipAddress, message.Status == IPStatus.Success);
                _logger.Info($"Sending online status message back to sender. Online status: {onlineStatusMessage.IpAddress.Value} is online:  {onlineStatusMessage.IsOnline}");
                Sender.Tell(onlineStatusMessage);
            });
            ipAddressResult.IfFail(exception => { _logger.Error(exception.Message);});
        }
    }
}
