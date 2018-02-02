using System;
using Akka.Actor;
using trondr.OpTools.Library.Infrastructure;
using trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Messages;

namespace trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Actors
{
    public class ScanFolderActor: ReceiveActor
    {
        public ScanFolderActor()
        {
            Receive<ScanFolderMessage>(message => OnHandleScanFolderMessage(message));
        }

        private void OnHandleScanFolderMessage(ScanFolderMessage message)
        {
            ToDo.Implement(ToDoPriority.Critical,"trondr","Handle ScanFolderMessage.");
            Context.Stop(Context.Self);
        }
    }
}