namespace trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Messages
{
    internal class UsageWriterActorOpenMessage
    {
        public string LocalDataFile { get; }
        public string UploadDataFile { get; }

        public UsageWriterActorOpenMessage(string localDataFile, string uploadDataFile)
        {
            LocalDataFile = localDataFile;
            UploadDataFile = uploadDataFile;
        }
    }
}