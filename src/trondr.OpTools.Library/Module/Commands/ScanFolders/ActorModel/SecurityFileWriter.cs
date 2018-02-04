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
            _sw.WriteLine(GetCsvHeader());
        }

        public void WriteSecurityRecord(SecurityRecordMessage securityRecordMessage)
        {
            _sw.WriteLine(GetCsvLine(securityRecordMessage.Hostname, securityRecordMessage.Path, securityRecordMessage.Accesscontroltype, securityRecordMessage.Identity, securityRecordMessage.Accessmask, securityRecordMessage.IsInherited, securityRecordMessage.Inheritanceflags, securityRecordMessage.Propagationflags));
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

        private string GetCsvHeader()
        {
            return GetCsvLine("Hostname", "Path", "AccessControlType", "Identity", "AccessMask", "IsInherited","InheritanceFlags","PropagationFlags");
        }

        private string GetCsvLine(string hostname, string path, string accesscontroltype, string identity, string accessmask, string isinherited, string inheritanceflags, string propagationflags)
        {
            return $"{Q(hostname)};{Q(path)};{Q(path)};{Q(accesscontroltype)};{Q(identity)};{Q(accessmask)};{Q(isinherited)};{Q(inheritanceflags)};{Q(propagationflags)}";
        }

        private string Q(string value)
        {
            return $"\"{value}\"";
        }
    }
}