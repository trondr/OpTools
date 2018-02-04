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
        private bool _overWrite;


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
                _sw.WriteLine(GetCsvLine(message.Hostname, message.Size, message.Path, message.Sddl,message.IsProtected ,message.Comment));                
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
            if (File.Exists(_uploadDataFile) && !_overWrite)
            {
                OnFailed($"Upload data file '{_uploadDataFile}' allready exists. Closing writer.", 0x000000B7);
                return;
            }
            File.Copy(_localDataFile,_uploadDataFile, _overWrite);
            Logger.Info($"Finished uploading '{_localDataFile}' -> '{_uploadDataFile}'!");
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
            _sw = new StreamWriter(_localDataFile);
            _sw.WriteLine(GetCsvHeader());
            _sw.AutoFlush = true;
            Become(LocalDataFileOpened);
        }

        private void OnClose()
        {
            CloseFile(ref _sw);
            Become(LocalDataFileClosed);            
        }

        private void CloseFile(ref StreamWriter sw)
        {
            if (sw == null) return;
            Logger.Info($"Closing local data file '{_localDataFile}'");
            sw.Close();
            sw.Dispose();
            sw = null;            
        }

        protected override void PostStop()
        {
            CloseFile(ref _sw);
            Logger.Info($"{GetType().Name}({Self.Path}) has stopped.");
            base.PostStop();
        }

        private string GetCsvHeader()
        {
            return GetCsvLine("Hostname","Size","Path","Sddl","IsProtected","Comment");
        }

        private string GetCsvLine(string hostName, string size, string path, string sddl, string isProtected,
            string comment)
        {
            return $"{Q(hostName)};{Q(size)};{Q(path)};{Q(sddl)};{Q(isProtected)};{Q(comment)}";
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