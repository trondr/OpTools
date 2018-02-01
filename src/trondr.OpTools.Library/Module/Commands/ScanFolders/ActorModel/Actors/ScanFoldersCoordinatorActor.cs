using Akka.Actor;
using trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Messages;

namespace trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Actors
{
    public class ScanFoldersCoordinatorActor : ReceiveActor
    {
        public ScanFoldersCoordinatorActor()
        {
            Receive<ScanFoldersMessage>(message => HandleScanFoldersMessage(message));
        }

        private void HandleScanFoldersMessage(ScanFoldersMessage message)
        {
            throw new System.NotImplementedException();
        }
    }
}