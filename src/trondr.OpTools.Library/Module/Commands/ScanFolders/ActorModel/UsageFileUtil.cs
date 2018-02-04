using System.IO;
using trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Messages;

namespace trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel
{
    public static class UsageFileUtil
    {
        public static string GetCsvHeader()
        {
            return GetCsvLine("Hostname", "Size", "Path", "Sddl", "IsProtected", "Comment");
        }

        public static string GetCsvLine(string hostName, string size, string path, string sddl, string isProtected,
            string comment)
        {
            return $"{Q(hostName)};{Q(size)};{Q(path)};{Q(sddl)};{Q(isProtected)};{Q(comment)}";
        }

        private static string Q(string value)
        {
            return $"\"{value}\"";
        }

        public static UsageRecordMessage ParseRecord(string csvLine)
        {
            var values = csvLine.Split(';');
            if (values.Length != 6)
                throw new InvalidDataException($"Csv line '{csvLine}' is not on the expected format of 6 columns: '{GetCsvHeader()}'");
            return new UsageRecordMessage(values[0].Trim('"'), values[1].Trim('"'), values[2].Trim('"'), values[3].Trim('"'), values[4].Trim('"'), values[5].Trim('"'));
        }

        public static void ParseHeader(string csvHeaderLine)
        {
            var expectedHeader = GetCsvHeader();            
            if (csvHeaderLine != expectedHeader)
            {
                throw new InvalidDataException($"Header in was expected to be '{expectedHeader}' but was '{csvHeaderLine}'");
            }
        }
    }
}