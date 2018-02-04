using System;
using System.IO;
using trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Messages;

namespace trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel
{
    public class SecurityFileWriter: IDisposable
    {
        public string FileName { get; }
        private StreamWriter _sw;

        public SecurityFileWriter(string fileName)
        {
            FileName = fileName;
            _sw = new StreamWriter(FileName);
            _sw.WriteLine(SecurityFileUtil.GetCsvHeader());
        }

        public void WriteSecurityRecord(SecurityRecordMessage securityRecordMessage)
        {
            _sw.WriteLine(SecurityFileUtil.GetCsvLine(securityRecordMessage.Hostname, securityRecordMessage.Path, securityRecordMessage.Accesscontroltype, securityRecordMessage.Identity, securityRecordMessage.Accessmask, securityRecordMessage.IsInherited, securityRecordMessage.Inheritanceflags, securityRecordMessage.Propagationflags));
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

        ~SecurityFileWriter()
        {
            ReleaseUnmanagedResources();
        }
    }
}