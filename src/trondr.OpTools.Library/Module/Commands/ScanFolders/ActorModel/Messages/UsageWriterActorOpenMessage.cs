namespace trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Messages
{
    internal class UsageWriterActorOpenMessage
    {
        public string LocalDataFile { get; }
        public string UploadDataFile { get; }
        public bool OverWrite { get; }

        public UsageWriterActorOpenMessage(string localDataFile, string uploadDataFile, bool overWrite)
        {
            LocalDataFile = localDataFile;
            UploadDataFile = uploadDataFile;
            OverWrite = overWrite;
        }
    }
}