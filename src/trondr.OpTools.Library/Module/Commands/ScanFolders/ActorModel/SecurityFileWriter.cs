using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Messages;

namespace trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel
{
    public class SecurityFileWriter: IDisposable
    {
        public string FileName { get; }
        private readonly StreamWriter _sw;
        private readonly CsvWriter _csvWriter;

        public SecurityFileWriter(string fileName)
        {
            FileName = fileName;
            _sw = new StreamWriter(FileName);
            var config = new Configuration
            {
                Delimiter = ";",
                PrepareHeaderForMatch = header => header.ToLower()
            };
            _csvWriter = new CsvWriter(_sw, config);
            _csvWriter.WriteHeader<SecurityRecordMessage>();
            _csvWriter.NextRecord();
        }

        public void WriteSecurityRecord(SecurityRecordMessage securityRecordMessage)
        {
            _csvWriter.WriteRecord(securityRecordMessage);
            _csvWriter.NextRecord();
        }

        public void WriteRecords(IEnumerable<SecurityRecordMessage> securityRecordMessages)
        {
            _csvWriter.WriteRecords(securityRecordMessages);
        }

        private void ReleaseUnmanagedResources()
        {
            _csvWriter?.Dispose();
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