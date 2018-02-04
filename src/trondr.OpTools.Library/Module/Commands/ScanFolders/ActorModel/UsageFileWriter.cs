using System;
using System.IO;
using trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Messages;

namespace trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel
{
    public class UsageFileWriter : IDisposable
    {
        public string FileName { get; }
        private StreamWriter _sw;

        public UsageFileWriter(string fileName)
        {
            FileName = fileName;
            _sw = new StreamWriter(FileName);
            _sw.AutoFlush = true;
            _sw.WriteLine(UsageFileUtil.GetCsvHeader());
        }

        public void WriteRecord(UsageRecordMessage securityRecordMessage)
        {
            _sw.WriteLine(UsageFileUtil.GetCsvLine(securityRecordMessage.Hostname, securityRecordMessage.Size, securityRecordMessage.Path, securityRecordMessage.Sddl, securityRecordMessage.IsProtected, securityRecordMessage.Comment));
        }

        private void ReleaseUnmanagedResources()
        {
            _sw?.Close();
            _sw?.Dispose();
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~UsageFileWriter()
        {
            ReleaseUnmanagedResources();
        }        
    }
}