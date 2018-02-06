using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Messages;

namespace trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel
{
    public class UsageFileReader : IDisposable
    {
        public string FileName { get; }

        public UsageFileReader(string fileName)
        {
            FileName = fileName;
        }

        public IEnumerable<UsageRecordMessage> GetAllRecords()
        {
            using (var sr = new StreamReader(FileName))
            {
                var config = new Configuration
                {
                    Delimiter = ";",
                    HasHeaderRecord = true,
                    PrepareHeaderForMatch = header => header.ToLower(),
                };
                using (var csvReader = new CsvReader(sr, config))
                {
                    foreach (var record in csvReader.GetRecords<UsageRecordMessage>())
                    {
                        yield return record;
                    }
                }
            }
        }

        private void ReleaseUnmanagedResources()
        {

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