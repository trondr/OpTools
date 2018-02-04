namespace trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Messages
{
    public class SecurityWriterActorOpenMessage
    {
        public string LocalSecurityDataFile { get; }
        public string UploadSecurityDataFile { get; }
        public bool Overwrite { get; }

        public SecurityWriterActorOpenMessage(string localSecurityDataFile, string uploadSecurityDataFile, bool overwrite)
        {
            LocalSecurityDataFile = localSecurityDataFile;
            UploadSecurityDataFile = uploadSecurityDataFile;
            Overwrite = overwrite;
        }
    }
}