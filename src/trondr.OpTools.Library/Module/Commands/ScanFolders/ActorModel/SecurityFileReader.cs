using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Messages;

namespace trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel
{
    public class SecurityFileReader : IDisposable
    {
        public string FileName { get; }
        
        public SecurityFileReader(string fileName)
        {
            FileName = fileName;            
        }

        public IEnumerable<SecurityRecordMessage> ReadAllSecurityRecord()
        {
            using (var sr = new StreamReader(FileName))
            {
                var config = new Configuration
                {
                    Delimiter = ";",
                    HasHeaderRecord = true,
                    PrepareHeaderForMatch = header => header.ToLower()
                };
                using (var csvReader = new CsvReader(sr, config))
                {
                    foreach (var record in csvReader.GetRecords<SecurityRecordMessage>())
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

        ~SecurityFileReader()
        {
            ReleaseUnmanagedResources();
        }        
    }
}