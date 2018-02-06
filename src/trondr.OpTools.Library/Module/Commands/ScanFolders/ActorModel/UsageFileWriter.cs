using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Messages;

namespace trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel
{
    public class UsageFileWriter : IDisposable
    {
        public string FileName { get; }
        private StreamWriter _sw;
        private CsvWriter _csvWriter;

        public UsageFileWriter(string fileName)
        {
            FileName = fileName;
            _sw = new StreamWriter(FileName) {AutoFlush = true};
            var config = new Configuration
            {
                Delimiter = ";",
                PrepareHeaderForMatch = header => header.ToLower()
            };
            _csvWriter = new CsvWriter(_sw,config);
            _csvWriter.WriteHeader<UsageRecordMessage>();
            _csvWriter.NextRecord();
        }

        public void WriteRecord(UsageRecordMessage usageRecordMessage)
        {
            _csvWriter.WriteRecord(usageRecordMessage);
            _csvWriter.NextRecord();
        }

        public void WriteRecords(IEnumerable<UsageRecordMessage> usageRecordMessages)
        {
            _csvWriter.WriteRecords(usageRecordMessages);            
        }

        private void ReleaseUnmanagedResources()
        {
            _csvWriter?.Dispose();
            _csvWriter = null;
            _sw?.Close();
            _sw?.Dispose();
            _sw = null;
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