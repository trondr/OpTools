using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using Akka.DI.Core;
using Akka.Event;
using trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Messages;

namespace trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Actors
{
    public class ScanFoldersCoordinatorActor : ReceiveActor
    {
        private ILoggingAdapter _logger;
        private ILoggingAdapter Logger => _logger ?? (_logger = Context.GetLogger());

        private readonly Dictionary<IActorRef, ScanFolderActorStatus> _folderScanActors;

        public ScanFoldersCoordinatorActor()
        {
            _folderScanActors = new Dictionary<IActorRef, ScanFolderActorStatus>();
            Receive<ScanFoldersMessage>(message => OnHandleScanFoldersMessage(message));
            Receive<StopScanFoldersCoordinatorActorMessage>(message => { OnStopScanFoldersCoordinatorActor(); });
            Receive<Terminated>(terminated => OnHandleTerminated(terminated));
        }

        private void OnStopScanFoldersCoordinatorActor()
        {
            Logger.Info("Stopping ScanFoldersCoordinatorActor and actor system");
            Context.Stop(Context.Self);
            Context.System.Terminate();
        }

        private void OnHandleTerminated(Terminated terminated)
        {
            if (_folderScanActors.ContainsKey(terminated.ActorRef))
            {
                _folderScanActors[terminated.ActorRef] = ScanFolderActorStatus.Terminated;
            }

            var allActorsHasTerminated = _folderScanActors.Values.All(status => status == ScanFolderActorStatus.Terminated);
            if (allActorsHasTerminated)
            {
                Logger.Info("All scan folder actors has terminated. Send stop message to ScanFoldersCoordinatorActor.");
                Context.Self.Tell(new StopScanFoldersCoordinatorActorMessage());
            }
        }

        private void OnHandleScanFoldersMessage(ScanFoldersMessage message)
        {
            foreach (var uncPath in message.UncPathsToScan)
            {
                var scanFolderMessageResult = ScanFolderMessage.Create(uncPath);
                scanFolderMessageResult.IfSucc(scanFolderMessage =>
                {
                    var actorName = $"ScanFolderActor_{scanFolderMessage.Name}";
                    Logger.Info($"Creating scan folder actor '{actorName}'");
                    var props = Context.DI().Props<ScanFolderActor>();
                    var scanFolderActor = Context.ActorOf(props, actorName);
                    _folderScanActors.Add(scanFolderActor,ScanFolderActorStatus.Running);
                    scanFolderActor.Tell(scanFolderMessage);
                    Context.Watch(scanFolderActor);
                });
                scanFolderMessageResult.IfFail(exception =>
                {
                    Logger.Error($"Failed to create scan folder message. {exception.Message} Terminating.");
                    Context.Self.Tell(new StopScanFoldersCoordinatorActorMessage());
                });
            }
        }        
    }
}