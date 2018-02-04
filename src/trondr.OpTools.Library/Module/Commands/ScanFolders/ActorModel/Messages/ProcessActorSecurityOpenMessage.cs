using Akka.Actor;

namespace trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Messages
{
    public class ProcessActorSecurityOpenMessage
    {
        public ProcessActorSecurityOpenMessage(IActorRef coordinator)
        {
            Coordinator = coordinator;            
        }

        public IActorRef Coordinator { get; }        
    }
}