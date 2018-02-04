using Akka.Actor;
using Akka.Event;
using Pri.LongPath;
using trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Messages;

namespace trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Actors
{
    public class SecurityWriterActor : ReceiveActor
    {
        private ILoggingAdapter _logger;
        private ILoggingAdapter Logger => _logger ?? (_logger = Context.GetLogger());
        private SecurityFileWriter _securityFileWriter;
        private OnOpenSecurityWriterActorMessage _message;

        public SecurityWriterActor()
        {
            Become(Initializing);
        }

        #region Behaviours
        private void Initializing()
        {
            Receive<OnOpenSecurityWriterActorMessage>(message => { OnOpen(message); });
        }
        
        private void Opened()
        {
            Receive<OnOpenSecurityWriterActorMessage>(message => { Logger.Error($"{GetType().Name} has allready been initialized."); });
            Receive<SecurityRecordMessage>(message => { _securityFileWriter.WriteSecurityRecord(message); });
            Receive<OnCloseSecurityWriterActorMessage>(message => { OnClose(); });
            Receive<OnUploadSecurityWriterActorMessage>(message => Logger.Error($"Uable to upload security data file because {GetType().Name} has not been closed."));
        }

        private void Closed()
        {
            Receive<OnOpenSecurityWriterActorMessage>(message => { Logger.Error($"{GetType().Name} has been closed and cannot be reopened."); });
            Receive<SecurityRecordMessage>(message => { Logger.Error($"{GetType().Name} has been closed and cannot write more {typeof(SecurityRecordMessage).Namespace}'s."); });
            Receive<OnCloseSecurityWriterActorMessage>(message => { Logger.Error($"{GetType().Name} has allready been closed."); });
            Receive<OnUploadSecurityWriterActorMessage>(message => OnUpload());
        }
        #endregion

        #region Handlers
        private void OnClose()
        {
            CloseFile(ref _securityFileWriter);
            Become(Closed);
        }

        private void OnOpen(OnOpenSecurityWriterActorMessage message)
        {
            if (File.Exists(message.LocalSecurityDataFile) && !message.Overwrite)
            {
                OnFailed(
                    $"Cannot open for writing of usage records. Local data file '{message.LocalSecurityDataFile}' allready exists. Closing writer.",
                    0x000000B7);
                return;
            }

            _message = message;
            _securityFileWriter = new SecurityFileWriter(message.LocalSecurityDataFile);
            Become(Opened);
        }

        private void OnUpload()
        {
            Logger.Info($"Uploading '{_message.LocalSecurityDataFile}' -> '{_message.UploadSecurityDataFile}'...");
            if (File.Exists(_message.UploadSecurityDataFile) && !_message.Overwrite)
            {
                OnFailed($"Upload data file '{_message.UploadSecurityDataFile}' allready exists. Closing writer.",
                    0x000000B7);
                return;
            }

            File.Copy(_message.LocalSecurityDataFile, _message.UploadSecurityDataFile, _message.Overwrite);
            Logger.Info($"Finished uploading '{_message.LocalSecurityDataFile}' -> '{_message.UploadSecurityDataFile}'!");
        }

        private void OnFailed(string message, int exitCode)
        {
            Logger.Error(message);
            Context.Parent.Tell(new ActorFailedMessage(message, exitCode));
            Context.Stop(Self);
        }
        #endregion

        #region Methods
        private void CloseFile(ref SecurityFileWriter securityFileWriter)
        {
            if (securityFileWriter == null) return;
            Logger.Info($"Closing local data file '{_message?.LocalSecurityDataFile}'");
            securityFileWriter.Dispose();
            securityFileWriter = null;
        }
        #endregion

        protected override void PostStop()
        {
            CloseFile(ref _securityFileWriter);
            Logger.Info($"{GetType().Name}({Self.Path}) has stopped.");
            base.PostStop();
        }
    }  
}
