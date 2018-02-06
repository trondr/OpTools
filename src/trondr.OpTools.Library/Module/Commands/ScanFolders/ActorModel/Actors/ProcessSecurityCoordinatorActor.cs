using System;
using Akka.Actor;
using Akka.DI.Core;
using Akka.Event;
using trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Messages;

namespace trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Actors
{
    public class ProcessSecurityCoordinatorActor : ReceiveActor
    {
        private ILoggingAdapter _logger;
        private long _pending;
        private bool _checkIfDone;
        private long _total;
        private IActorRef _securityWriterActor;
        private ProcessSecurityCoordinatorActorOpenMessage _message;
        private IActorRef _processSecurityActor;
        private ILoggingAdapter Logger => _logger ?? (_logger = Context.GetLogger());


        public ProcessSecurityCoordinatorActor()
        {
            Become(Initializing);
        }

        private void Initializing()
        {
            Receive<ProcessSecurityCoordinatorActorOpenMessage>(message => OnOpen(message));
        }

        private void Started()
        {
            Receive<ProcessSecurityCoordinatorActorOpenMessage>(message => Logger.Error($"{GetType().Name} has allready started."));
            Receive<SecurityRecordMessage>(message => OnSave(message));
            Receive<UsageRecordProcessedMessage>(message =>
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
                    Self.Tell(new ProcessingIsDoneMessage());
                }
            });
            Receive<ProcessingIsDoneMessage>(message =>
            {
                Logger.Info($"All {typeof(ProcessSecurityActor).Name}'s has finished. Close and upload data and then request terminatation of the {typeof(SecurityWriterActor).Name}.");
                _securityWriterActor.Tell(new SecurityWriterActorCloseMessage());
                _securityWriterActor.Tell(new SecurityWriterActorUploadMessage());
                _securityWriterActor.Tell(PoisonPill.Instance);
            });
            Receive<LoadUsageRecordsMessage>(message => OnLoadUsageRecords());
            Receive<SecurityWriterActorTerminatedMessage>(message =>
            {
                Logger.Info($"{typeof(SecurityWriterActor).Name} has terminated. Request stop of {typeof(ProcessSecurityCoordinatorActor).Name}");
                Context.Self.Tell(new ProcessSecurityCoordinatorActorStopMessage());
            });
            Receive<ProcessSecurityCoordinatorActorStopMessage>(message =>
            {
                Logger.Info($"Stopping {GetType().Name}...");
                Context.Self.Tell(PoisonPill.Instance);
            });
            Receive<ActorFailedMessage>(message =>
            {
                Logger.Error(message.Message);
                _message.ExitCode = message.ExitCode;
                Context.Self.Tell(new ProcessSecurityCoordinatorActorStopMessage());
            });
        }
        
        private void OnOpen(ProcessSecurityCoordinatorActorOpenMessage message)
        {
            _message = message;
            _securityWriterActor = CreateAndStartSecurityWriterActor(message.LocalDataFile, message.UploadDataFile, message.OverWrite);
            _processSecurityActor = CreateAndStartProcessSecurityActor();
            Become(Started);
            Self.Tell(new LoadUsageRecordsMessage());            
        }

        private IActorRef CreateAndStartProcessSecurityActor()
        {
            var actor = Context.ActorOf(Context.DI().Props<ProcessSecurityActor>());
            actor.Tell(new ProcessActorSecurityOpenMessage(Self));
            Context.WatchWith(actor, new SecurityWriterActorTerminatedMessage());
            return actor;
        }

        private IActorRef CreateAndStartSecurityWriterActor(string localDataFile, string uploadDataFile, bool overWrite)
        {
            var actor = Context.ActorOf(Context.DI().Props<SecurityWriterActor>());
            actor.Tell(new SecurityWriterActorOpenMessage(localDataFile, uploadDataFile, overWrite));
            Context.WatchWith(actor, new SecurityWriterActorTerminatedMessage());
            return actor;
        }

        private void OnSave(SecurityRecordMessage message)
        {
            _securityWriterActor.Tell(message);
        }

        private void OnLoadUsageRecords()
        {
            using (var usageFileReader = new UsageFileReader(_message.LocalUsageDataFile))
            {
                foreach (var usageRecordMessage in usageFileReader.GetAllRecords())
                {
                    _pending++;
                    _total++;
                    _processSecurityActor.Tell(usageRecordMessage);
                }
            }
        }

        protected override void PostStop()
        {
            Logger.Info($"{GetType().Name}({Self.Path}) has stopped.");
            base.PostStop();
        }
    }
}
