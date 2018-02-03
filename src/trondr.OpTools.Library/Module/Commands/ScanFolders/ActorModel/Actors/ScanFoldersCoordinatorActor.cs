using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.DI.Core;
using Akka.Event;
using Akka.Routing;
using trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Messages;

namespace trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Actors
{
    public class ScanFoldersCoordinatorActor : ReceiveActor
    {
        private ILoggingAdapter _logger;
        private ILoggingAdapter Logger => _logger ?? (_logger = Context.GetLogger());

        private IActorRef _usageWriterActor;
        private ScanFoldersMessage _scanFoldersMessage;

        public ScanFoldersCoordinatorActor()
        {        
            Become(Ready);
        }

        private void Ready()
        {
            Receive<ScanFoldersMessage>(message => OnHandleScanFoldersMessage(message));            
        }

        private void Busy()
        {
            Receive<ScanFoldersMessage>(message =>
            {
                Logger.Error($"Unable to start more folder scans. {typeof(ScanFoldersCoordinatorActor).Name} is busy.");
            });
            Receive<StopScanFoldersCoordinatorActorMessage>(message => { OnStopScanFoldersCoordinatorActor(); });
            Receive<ScanFolderActorsTerminatedMessage>(message =>
            {
                Logger.Info($"All {typeof(ScanFolderActor).Name}'s has terminated. Close and upload data and then request terminatation of the {typeof(UsageWriterActor).Name}.");
                _usageWriterActor.Tell(new UsageWriterActorCloseMessage());
                _usageWriterActor.Tell(new UsageWriterActorUploadMessage());
                _usageWriterActor.Tell(PoisonPill.Instance);
            });
            Receive<UsageWriterActorTerminatedMessage>(message =>
            {
                Logger.Info($"{typeof(UsageWriterActor).Name} has terminated. Request stop of {typeof(ScanFoldersCoordinatorActor).Name}");
                Context.Self.Tell(new StopScanFoldersCoordinatorActorMessage());
            });
            Receive<ActorFailedMessage>(message =>
            {
                Logger.Error(message.Message);
                _scanFoldersMessage.ExitCode = message.ExitCode;
                Context.Self.Tell(new StopScanFoldersCoordinatorActorMessage());
            });
        }
        
        private void OnStopScanFoldersCoordinatorActor()
        {
            Logger.Info($"Stopping {GetType().Name}...");
            Context.Self.Tell(PoisonPill.Instance);            
        }

        private void OnHandleScanFoldersMessage(ScanFoldersMessage message)
        {
            Become(Busy);
            _scanFoldersMessage = message;
            _usageWriterActor = Context.ActorOf(Context.DI().Props<UsageWriterActor>());
            var localDataFile = Path.Combine(message.LocalDataFolder, "FolderUsage.csv");
            var uploadDataFile = Path.Combine(message.UploadDataFolder, "FolderUsage.csv"); ;
            _usageWriterActor.Tell(new UsageWriterActorOpenMessage(localDataFile, uploadDataFile));
            Context.WatchWith(_usageWriterActor, new UsageWriterActorTerminatedMessage());

            var roundRobinScanFolderActorRouter = CreateScanFolderActorRouter(message.UncPathsToScan.Length);
            foreach (var uncPath in message.UncPathsToScan)
            {
                var scanFolderMessageResult = ScanFolderMessage.Create(uncPath, _usageWriterActor);
                scanFolderMessageResult.IfSucc(scanFolderMessage => SendScanFolderMessage(scanFolderMessage, roundRobinScanFolderActorRouter));
                scanFolderMessageResult.IfFail(exception =>
                {
                    Logger.Error($"Failed to create scan folder message. {exception.Message} Terminating.");
                    Context.Self.Tell(new StopScanFoldersCoordinatorActorMessage());
                });
            }
        }

        private IActorRef CreateScanFolderActorRouter(int numberOfUncPathsToScan)
        {
            var roundRobinScanFolderActorRouter = Context.ActorOf(Context.DI().Props<ScanFolderActor>().WithRouter(new RoundRobinPool(numberOfUncPathsToScan)));
            Logger.Info($"Watch {typeof(RoundRobinPool).Name} of {typeof(ScanFolderActor).Name}'s for termination.");
            Context.WatchWith(roundRobinScanFolderActorRouter, new ScanFolderActorsTerminatedMessage());
            return roundRobinScanFolderActorRouter;
        }

        private void SendScanFolderMessage(ScanFolderMessage scanFolderMessage, IActorRef roundRobinScanFolderActorRouter)
        {
            Logger.Info($"Send {typeof(ScanFolderMessage).Name}({scanFolderMessage.UncPath}) to {typeof(ScanFolderActor).Name}'s");
            roundRobinScanFolderActorRouter.Tell(scanFolderMessage);            
        }

        protected override void PostStop()
        {
            Logger.Info($"{GetType().Name} '{Self.Path}' has stopped.");
            var system = Context.System;
            Logger.Info($"Stopping actor system in 3 seconds...");
            Task.Delay(3000).ContinueWith(task => system.Terminate());
            base.PostStop();
        }
    }
}