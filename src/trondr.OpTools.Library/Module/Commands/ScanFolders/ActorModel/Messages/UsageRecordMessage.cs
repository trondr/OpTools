namespace trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Messages
{
    public class UsageRecordMessage
    {
        public UsageRecordMessage(string hostname, string size, string path, string sddl, string isProtected,
            string comment)
        {
            Size = size;
            Path = path;
            Sddl = sddl;
            IsProtected = isProtected;
            Comment = comment;
            Hostname = hostname;
        }

        public string Hostname { get; }
        public string Size { get; }
        public string Path { get; }
        public string Sddl { get; }
        public string IsProtected { get; }
        public string Comment { get; }
    }
}