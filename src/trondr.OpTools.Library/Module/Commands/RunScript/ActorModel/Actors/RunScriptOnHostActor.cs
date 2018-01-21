using System.Threading;
using Akka.Actor;
using Akka.Event;
using trondr.OpTools.Library.Module.Commands.RunScript.ActorModel.Messages;

namespace trondr.OpTools.Library.Module.Commands.RunScript.ActorModel.Actors
{
    public class RunScriptOnHostActor : ReceiveActor
    {
        private ILoggingAdapter _logger;
        private ILoggingAdapter Logger => _logger ?? (_logger = Context.GetLogger());

        public RunScriptOnHostActor()
        {
            Receive<RunScriptOnHostMessage>(message => HandleRunScriptOnHostMessage(message));
        }

        private void HandleRunScriptOnHostMessage(RunScriptOnHostMessage message)
        {
            Logger.Warning($"Simulating running script '{message.ScriptPath}' on {message.Host.HostName}");
            Thread.Sleep(2000);
        }
    }
}