using System;
using Akka.Actor;
using Akka.Event;
using trondr.OpTools.Library.Infrastructure;
using trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Messages;

namespace trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Actors
{
    public class ScanFolderActor: ReceiveActor
    {
        private ILoggingAdapter _logger;
        private ILoggingAdapter Logger => _logger ?? (_logger = Context.GetLogger());

        public ScanFolderActor()
        {
            Receive<ScanFolderMessage>(message => OnHandleScanFolderMessage(message));
        }

        private void OnHandleScanFolderMessage(ScanFolderMessage message)
        {
            try
            {
                ToDo.Implement(ToDoPriority.Critical, "trondr", "Handle ScanFolderMessage.");
                message.UsageWriterActor.Tell(new UsageRecordMessage(message.HostName, 1023.ToString(), message.UncPath,
                    "", "Some comment"));
                Logger.Info($"Done scanning '{message.UncPath}'. Terminating {typeof(ScanFolderActor).Name}");
            }
            catch (UnauthorizedAccessException ex)
            {
                Context.Parent.Tell(new ActorFailedMessage($"Failed to scan folder '{message.UncPath}'. {ex.Message}", 5));
            }
            catch (Exception ex)
            {
                Context.Parent.Tell(new ActorFailedMessage($"Failed to scan folder '{message.UncPath}'. {ex.Message}", 1));
            }
            finally
            {
                Context.Stop(Context.Self);
            }
        }

        protected override void PostStop()
        {
            Logger.Info($"{GetType().Name} '{Self.Path}' has stopped.");
            base.PostStop();
        }
    }
}