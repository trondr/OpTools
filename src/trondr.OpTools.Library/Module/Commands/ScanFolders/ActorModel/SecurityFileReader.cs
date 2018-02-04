using System;
using System.Collections.Generic;
using System.IO;
using trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Messages;

namespace trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel
{
    public class SecurityFileReader : IDisposable
    {
        public string FileName { get; }
        private readonly StreamReader _sr;

        public SecurityFileReader(string fileName)
        {
            FileName = fileName;
            _sr = new StreamReader(FileName);
            SecurityFileUtil.ParseHeader(_sr.ReadLine());
        }

        public IEnumerable<SecurityRecordMessage> ReadAllSecurityRecord()
        {
            while (_sr.Peek() >= 0)
            {
                yield return SecurityFileUtil.ParseRecord(_sr.ReadLine());
            }            
        }

        private void ReleaseUnmanagedResources()
        {
            _sr?.Close();
            _sr?.Dispose();
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~SecurityFileReader()
        {
            ReleaseUnmanagedResources();
        }        
    }
}