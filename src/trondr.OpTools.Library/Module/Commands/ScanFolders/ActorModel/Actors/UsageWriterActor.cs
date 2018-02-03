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
        private StreamWriter _sw;
        private string _uploadDataFile;


        public UsageWriterActor()
        {            
            Become(ReadyForOpeningOfLocalDataFile);
        }

        private void ReadyForOpeningOfLocalDataFile()
        {
            Receive<UsageWriterActorOpenMessage>(message => OnOpen(message));
            Receive<UsageWriterActorCloseMessage>(message =>
            {
                Logger.Error($"Cannot close {typeof(UsageWriterActor).Name} as it has not been previously been opened.");
            });            
            Receive<UsageRecordMessage>(message =>
            {
                Logger.Error($"Local data file has not been created. Dropping usage record: {message.Path}");
            });
        }

        private void LocalDataFileOpened()
        {
            Receive<UsageWriterActorOpenMessage>(message =>
            {
                Logger.Error($"Local data file is allready open");
            });
            Receive<UsageWriterActorCloseMessage>(message => { OnClose(); });
            Receive<UsageRecordMessage>(message =>
            {
                _sw.WriteLine(GetCsvLine(message.Hostname, message.Size, message.Path, message.Sddl, message.Comment));                
            });
            Receive<UsageWriterActorUploadMessage>(message =>
            {
                Logger.Error($"Unable to upload local data file '{_localDataFile}' as it is still open.");
            });
        }

        private void LocalDataFileClosed()
        {            
            Receive<UsageWriterActorOpenMessage>(message =>
            {
                Logger.Error($"Cannot open for writing of usage records. Local data file '{message.LocalDataFile}' allready exists.");
            });
            Receive<UsageRecordMessage>(message =>
            {
                Logger.Error($"Local data file is closed. Dropping usage record: {message.Path}");
            });
            Receive<UsageWriterActorUploadMessage>(message => OnUpload(message));
        }

        private void OnUpload(UsageWriterActorUploadMessage message)
        {
            Logger.Info($"Uploading '{_localDataFile}' -> '{_uploadDataFile}'...");
            if (File.Exists(_uploadDataFile))
            {
                OnFailed($"Upload data file '{_uploadDataFile}' allready exists. Closing writer.", 0x000000B7);
                return;
            }
            File.Copy(_localDataFile,_uploadDataFile);
            Logger.Info($"Finished uploading '{_localDataFile}' -> '{_uploadDataFile}'!");
        }

        private void OnOpen(UsageWriterActorOpenMessage message)
        {
            _localDataFile = message.LocalDataFile;
            _uploadDataFile = message.UploadDataFile;
            Logger.Info($"Checking if data file '{_localDataFile}' allready exists.");
            if (File.Exists(_localDataFile))
            {
                OnFailed($"Cannot open for writing of usage records. Local data file '{_localDataFile}' allready exists. Closing writer.", 0x000000B7);
                return;
            }
            Logger.Info($"Opening output data file '{_localDataFile}'");
            _sw = new StreamWriter(_localDataFile);
            _sw.WriteLine(GetCsvHeader());
            _sw.AutoFlush = true;
            Become(LocalDataFileOpened);
        }

        private void OnClose()
        {
            _sw?.Close();
            _sw?.Dispose();
            Become(LocalDataFileClosed);            
        }

        protected override void PostStop()
        {
            if (_sw != null)
            {
                Logger.Info($"Closing local data file '{_localDataFile}'");
                _sw?.Close();
                _sw?.Dispose();
            }
            base.PostStop();
        }

        private string GetCsvHeader()
        {
            return GetCsvLine("Hostname","Size","Path","Sddl","Comment");
        }

        private string GetCsvLine(string hostName, string size, string path, string sddl, string comment)
        {
            return $"{Q(hostName)};{Q(size)};{Q(path)};{Q(sddl)};{Q(comment)}";
        }

        private string Q(string value)
        {
            return $"\"{value}\"";
        }


        private void OnFailed(string message, int exitCode)
        {
            Logger.Error(message);
            Context.Parent.Tell(new ActorFailedMessage(message, exitCode));
            Context.Stop(Self);            
        }
    }
}