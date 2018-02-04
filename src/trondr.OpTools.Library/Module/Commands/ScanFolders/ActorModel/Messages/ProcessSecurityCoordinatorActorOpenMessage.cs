namespace trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Messages
{
    public class ProcessSecurityCoordinatorActorOpenMessage
    {
        public ProcessSecurityCoordinatorActorOpenMessage(string localDataFile, string uploadDataFile, bool overWrite, string localUsageDataFile)
        {
            LocalDataFile = localDataFile;
            UploadDataFile = uploadDataFile;
            OverWrite = overWrite;
            LocalUsageDataFile = localUsageDataFile;
            ExitCode = 0;
        }

        public string LocalDataFile { get; }
        public string UploadDataFile { get; }
        public bool OverWrite { get; }
        public string LocalUsageDataFile { get; }
        public int ExitCode { get; set; }
    }
}