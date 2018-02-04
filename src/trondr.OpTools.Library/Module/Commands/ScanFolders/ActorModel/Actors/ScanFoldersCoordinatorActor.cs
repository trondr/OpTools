using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Configuration;
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
        private long _pending;
        private bool _checkIfDone;
        private long _total;
        private IActorRef _processFolderActorRouter;

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
            Receive<ProcessFolderMessage>(message =>
            {
                _pending++;
                _total++;
                _checkIfDone = false;
                Console.Write($"Pending: {_pending} of {_total}\t\r");
                _processFolderActorRouter.Tell(message);
            });
            Receive<ProcessedFolderMessage>(message =>
            {
                _pending--;
                if (_pending > 0) return;
                _checkIfDone = true;
                var self = Self;
                Console.Write($"Pending: {_pending} of {_total}\t\r");
                self.Tell(new CheckIfDoneMessage());                
            });
            Receive<CheckIfDoneMessage>(message =>
            {
                Logger.Info("Checking if done...");
                if (_pending <= 0 && _checkIfDone)
                {
                    Logger.Info($"Done! {_pending} pending folders of total {_total}. Request termination.");
                    Self.Tell(new ProcessFolderActorsTerminatedMessage());
                }
            });
            Receive<StopScanFoldersCoordinatorActorMessage>(message => 
            {
                Logger.Info($"Stopping {GetType().Name}...");
                Context.Self.Tell(PoisonPill.Instance);
            });
            Receive<ProcessFolderActorsTerminatedMessage>(message =>
            {
                Logger.Info($"All {typeof(ProcessFolderMessage).Name}'s has terminated. Close and upload data and then request terminatation of the {typeof(UsageWriterActor).Name}.");
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
            _processFolderActorRouter = CreateProcessFolderActorRouter(message.DegreeOfParallelism);
            var scanFoldersActors = new ScanFoldersActors(Self,_processFolderActorRouter,_usageWriterActor);
            var exitCode = StartFolderScanning(message.UncPathsToScan, scanFoldersActors);
            _scanFoldersMessage.ExitCode = exitCode;
        }

        private IActorRef CreateProcessFolderActorRouter(int degreeOfParallelism)
        {
            var processFolderActorRouter = Context.ActorOf(Context.DI().Props<ProcessFolderActor>().WithRouter(new SmallestMailboxPool(degreeOfParallelism)));
            Logger.Info($"Watch {typeof(SmallestMailboxPool).Name} of {typeof(ProcessFolderActor).Name}'s for termination.");
            Context.WatchWith(processFolderActorRouter, new ProcessFolderActorsTerminatedMessage());
            return processFolderActorRouter;
        }

        private int StartFolderScanning(string[] uncPathsToScan, ScanFoldersActors scanFoldersActors)
        {
            var exitCode = 0;
            foreach (var uncPath in uncPathsToScan)
            {
                var processFolderMessageResult = ProcessFolderMessage.Create(uncPath, scanFoldersActors);
                processFolderMessageResult.IfSucc(processFolderMessage =>
                    {
                        Logger.Info($"Send {typeof(ProcessFolderMessage).Name}({processFolderMessage.UncPath}) to {typeof(ProcessFolderActor).Name}'s");
                        Self.Tell(processFolderMessage);
                    });
                processFolderMessageResult.IfFail(exception =>
                {
                    Logger.Error($"Failed to create {typeof(ProcessFolderMessage).Name} from path '{uncPath}' in {this.GetType().Namespace}.  {exception.Message}. Terminating");
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

        protected override void PostStop()
        {
            Logger.Info($"{GetType().Name}({Self.Path}) has stopped.");
            var system = Context.System;

            Logger.Info($"Stopping actor system in 5 seconds...");
            Task.Delay(5000).ContinueWith(task => system.Terminate());
            base.PostStop();
        }
    }
}