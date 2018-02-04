using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Messages;

namespace trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel
{
    public static class SecurityFileUtil
    {
        public static string GetCsvHeader()
        {
            return GetCsvLine("Hostname", "Path", "AccessControlType", "Identity", "AccessMask", "IsInherited", "InheritanceFlags", "PropagationFlags");
        }

        public static string GetCsvLine(string hostname, string path, string accesscontroltype, string identity, string accessmask, string isinherited, string inheritanceflags, string propagationflags)
        {
            return $"{Q(hostname)};{Q(path)};{Q(path)};{Q(accesscontroltype)};{Q(identity)};{Q(accessmask)};{Q(isinherited)};{Q(inheritanceflags)};{Q(propagationflags)}";
        }

        private static string Q(string value)
        {
            return $"\"{value}\"";
        }

        public static SecurityRecordMessage ParseRecord(string csvLine)
        {
            var values = csvLine.Split(';');
            if(values.Length != 8)
                throw new InvalidDataException($"Csv line '{csvLine}' is not on the expected format of 8 columns: '{GetCsvHeader()}'");
            return new SecurityRecordMessage(values[0].Trim('"'), values[1].Trim('"'), values[2].Trim('"'), values[3].Trim('"'), values[4].Trim('"'), values[5].Trim('"'), values[6].Trim('"'), values[7].Trim('"'));
        }

        public static void ParseHeader(string csvHeaderLine)
        {
            var expectedHeader = GetCsvHeader();
            if (csvHeaderLine != expectedHeader)
            {
                throw new InvalidDataException($"Header in was expected to be '{expectedHeader}' but was '{csvHeaderLine}'");
            }
        }

        public static IEnumerable<SecurityAccessRuleRecordMessage> GetSecurityAccessRuleRecords(string sddl)
        {
            if(string.IsNullOrWhiteSpace(sddl)) yield break;

            var directorySecurity = new DirectorySecurity();
            directorySecurity.SetSecurityDescriptorSddlForm(sddl);
            var accessRules = directorySecurity.GetAccessRules(true, true, typeof(NTAccount));
            foreach (FileSystemAccessRule rule in accessRules)
            {
                yield return new SecurityAccessRuleRecordMessage(rule.AccessControlType.ToString(), rule.IdentityReference.Value, rule.FileSystemRights.ToString(), rule.IsInherited.ToString(), rule.InheritanceFlags.ToString(), rule.PropagationFlags.ToString());
            }
        }
    }
}