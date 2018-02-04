using System;
using System.Collections.Generic;
using System.IO;
using trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Messages;

namespace trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel
{
    public class UsageFileReader : IDisposable
    {
        public string FileName { get; }
        private readonly StreamReader _sr;

        public UsageFileReader(string fileName)
        {
            FileName = fileName;
            _sr = new StreamReader(FileName);
            UsageFileUtil.ParseHeader(_sr.ReadLine());
        }

        public IEnumerable<UsageRecordMessage> GetAllRecords()
        {
            while (_sr.Peek() >= 0)
            {
                yield return UsageFileUtil.ParseRecord(_sr.ReadLine());
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

        ~UsageFileReader()
        {
            ReleaseUnmanagedResources();
        }
    }
}