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
                Logger.Error($"Unable to start more folder scans. {GetType().Name} is busy.");
            });
            Receive<StopScanFoldersCoordinatorActorMessage>(message => 
            {
                Logger.Info($"Stopping {GetType().Name}...");
                Context.Self.Tell(PoisonPill.Instance);
            });
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
        
        private void OnHandleScanFoldersMessage(ScanFoldersMessage message)
        {
            Become(Busy);
            _scanFoldersMessage = message;
            var localDataFile = Path.Combine(message.LocalDataFolder, "FolderUsage.csv");
            var uploadDataFile = Path.Combine(message.UploadDataFolder, "FolderUsage.csv");
            _usageWriterActor = CreateUsageWriterActor(localDataFile, uploadDataFile, message.OverWrite);
            var scanFolderActorRouter = CreateScanFolderActorRouter(message.UncPathsToScan.Length);
            var exitCode = StartFolderScanning(message.UncPathsToScan, scanFolderActorRouter, _usageWriterActor);
            _scanFoldersMessage.ExitCode = exitCode;
        }

        private int StartFolderScanning(string[] uncPathsToScan, IActorRef scanFolderActorRouter, IActorRef usageWriterActor)
        {
            var exitCode = 0;
            foreach (var uncPath in uncPathsToScan)
            {
                var scanFolderMessageResult = ScanFolderMessage.Create(uncPath, usageWriterActor);
                scanFolderMessageResult.IfSucc(scanFolderMessage => SendScanFolderMessage(scanFolderMessage, scanFolderActorRouter));
                scanFolderMessageResult.IfFail(exception =>
                {
                    Logger.Error($"Failed to create scan folder message. {exception.Message} Terminating.");
                    exitCode = 1;
                    Context.Self.Tell(new StopScanFoldersCoordinatorActorMessage());
                });
            }
            return exitCode;
        }
        
        private IActorRef CreateUsageWriterActor(string localDataFile, string uploadDataFile, bool overWrite)
        {
            var usageWriterActor = Context.ActorOf(Context.DI().Props<UsageWriterActor>());            
            usageWriterActor.Tell(new UsageWriterActorOpenMessage(localDataFile, uploadDataFile, overWrite));
            Logger.Info($"Watch {typeof(UsageWriterActor).Name} for termination.");
            Context.WatchWith(usageWriterActor, new UsageWriterActorTerminatedMessage());
            return usageWriterActor;
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

            Logger.Info($"Stopping actor system in 5 seconds...");
            Task.Delay(5000).ContinueWith(task => system.Terminate());
            base.PostStop();
        }
    }
}