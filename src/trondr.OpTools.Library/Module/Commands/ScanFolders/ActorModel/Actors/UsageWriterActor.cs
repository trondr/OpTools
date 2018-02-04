using System.IO;
using Akka.Actor;
using Akka.Event;
using trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Messages;

namespace trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Actors
{
    public class UsageWriterActor : ReceiveActor
    {
        private ILoggingAdapter _logger;
        private ILoggingAdapter Logger => _logger ?? (_logger = Context.GetLogger());

        private string _localDataFile;        
        
        private string _uploadDataFile;
        private bool _overWrite;
        private UsageFileWriter _usageFileWriter;


        public UsageWriterActor()
        {            
            Become(Initializing);
        }

        private void Initializing()
        {
            Receive<UsageWriterActorOpenMessage>(message => OnOpen(message));
            Receive<UsageWriterActorCloseMessage>(message => { Logger.Error($"Cannot close {typeof(UsageWriterActor).Name} as it has not been previously been opened."); });            
            Receive<UsageRecordMessage>(message => { Logger.Error($"Local data file has not been created. Dropping usage record: {message.Path}"); });
            Receive<UsageWriterActorUploadMessage>(message => { Logger.Error($"Unable to upload local data file '{_localDataFile}'. {GetType().Name} is in the Initializing phase.");
            });
        }

        private void Opened()
        {
            Receive<UsageWriterActorOpenMessage>(message => { Logger.Error($"Local data file {_localDataFile} is allready open"); });
            Receive<UsageWriterActorCloseMessage>(message => { OnClose(); });
            Receive<UsageRecordMessage>(message => { _usageFileWriter.WriteRecord(message); });
            Receive<UsageWriterActorUploadMessage>(message => { Logger.Error($"Unable to upload local data file '{_localDataFile}' as it is still open."); });
        }

        private void Closed()
        {            
            Receive<UsageWriterActorOpenMessage>(message => { Logger.Error($"Cannot open for writing of usage records. Local data file '{message.LocalDataFile}' is closed."); });
            Receive<UsageWriterActorCloseMessage>(message => { Logger.Error($"Cannot close {typeof(UsageWriterActor).Name} as it is allread closed."); });
            Receive<UsageRecordMessage>(message => { Logger.Error($"Local data file is closed. Dropping usage record: {message.Path}");});
            Receive<UsageWriterActorUploadMessage>(message => OnUpload());
        }

        private void OnOpen(UsageWriterActorOpenMessage message)
        {
            _localDataFile = message.LocalDataFile;
            _uploadDataFile = message.UploadDataFile;
            _overWrite = message.OverWrite;
            Logger.Info($"Checking if data file '{_localDataFile}' allready exists.");
            if (File.Exists(_localDataFile) && !_overWrite)
            {
                OnFailed($"Cannot open for writing of usage records. Local data file '{_localDataFile}' allready exists. Closing writer.", 0x000000B7);
                return;
            }
            Logger.Info($"Opening output data file '{_localDataFile}'");
            _usageFileWriter = new UsageFileWriter(_localDataFile);            
            Become(Opened);
        }

        private void OnClose()
        {
            CloseFile(ref _usageFileWriter);
            Become(Closed);            
        }

        private void OnUpload()
        {
            Logger.Info($"Uploading '{_localDataFile}' -> '{_uploadDataFile}'...");
            if (File.Exists(_uploadDataFile) && !_overWrite)
            {
                OnFailed($"Upload data file '{_uploadDataFile}' allready exists. Closing writer.", 0x000000B7);
                return;
            }
            File.Copy(_localDataFile, _uploadDataFile, _overWrite);
            Logger.Info($"Finished uploading '{_localDataFile}' -> '{_uploadDataFile}'!");
        }

        private void CloseFile(ref UsageFileWriter usageFileWriter)
        {
            if (usageFileWriter == null) return;
            Logger.Info($"Closing local data file '{_localDataFile}'");            
            usageFileWriter.Dispose();
            usageFileWriter = null;            
        }

        protected override void PostStop()
        {
            CloseFile(ref _usageFileWriter);
            Logger.Info($"{GetType().Name}({Self.Path}) has stopped.");
            base.PostStop();
        }

        private void OnFailed(string message, int exitCode)
        {
            Logger.Error(message);
            Context.Parent.Tell(new ActorFailedMessage(message, exitCode));
            Context.Stop(Self);            
        }
    }
}