using Akka.Actor;

namespace trondr.OpTools.Library.Module.Commands.ScanFolders
{
    public interface IScanFoldersCommandProvider
    {
        int ScanFolders(string[] uncPathsToScan, string localDataFolder, string uploadDataFolder, bool overWrite, int degreeOfParallelism, ActorSystem scanFoldersActorSystem);
    }
}