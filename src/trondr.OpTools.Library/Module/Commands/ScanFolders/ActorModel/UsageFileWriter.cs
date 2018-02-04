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
            _sw.WriteLine(GetCsvHeader());
        }

        public void WriteRecord(UsageRecordMessage securityRecordMessage)
        {
            _sw.WriteLine(GetCsvLine(securityRecordMessage.Hostname, securityRecordMessage.Size, securityRecordMessage.Path, securityRecordMessage.Sddl, securityRecordMessage.IsProtected, securityRecordMessage.Comment));
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

        private string GetCsvHeader()
        {
            return GetCsvLine("Hostname", "Size", "Path", "Sddl", "IsProtected", "Comment");
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
    }
}