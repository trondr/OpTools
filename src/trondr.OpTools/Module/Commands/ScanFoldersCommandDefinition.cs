using Akka.Actor;
using Akka.DI.CastleWindsor;
using Akka.DI.Core;
using Castle.Windsor;
using NCmdLiner.Attributes;
using trondr.OpTools.Library.Infrastructure;
using trondr.OpTools.Library.Module.Commands.ScanFolders;

namespace trondr.OpTools.Module.Commands
{
    public class ScanFoldersCommandDefinition: CommandDefinition
    {
        private readonly IScanFoldersCommandProviderFactory _scanFoldersCommandProviderFactory;
        private readonly IWindsorContainer _windsorContainer;

        public ScanFoldersCommandDefinition(IScanFoldersCommandProviderFactory scanFoldersCommandProviderFactory, IWindsorContainer windsorContainer)
        {
            _scanFoldersCommandProviderFactory = scanFoldersCommandProviderFactory;
            _windsorContainer = windsorContainer;
        }

        [Command(Description = "Scan folder usage and folder security. Result is output to Folder-Usage-<hostname>.csv and Folder-Security-<hostname>.csv. Hostname is derived from the unc paths.")]
        public int ScanFolders(
            [RequiredCommandParameter(Description = "Folders to scan.", AlternativeName = "fts", ExampleValue = new[]{@"\\localhost\c$\temp",@"\\localhost\c$\ProgramFiles"})]
            string[] uncPathsToScan,
            [RequiredCommandParameter(Description = "The result will be written to the data folder during scan.", AlternativeName = "df", ExampleValue = @"c:\temp\data")]
            string localDataFolder,
            [RequiredCommandParameter(Description = "The result will be uploaded to this folder when scan is done.", AlternativeName = "uf", ExampleValue = @"\\dataserver1\data\folderreports")]
            string uploadDataFolder
            )
        {

            using (var scanFoldersActorSystem = ActorSystem.Create("ScanFoldersActorSystem"))
            {
                var windsorDependencyResolver = new WindsorDependencyResolver(_windsorContainer, scanFoldersActorSystem);
                scanFoldersActorSystem.AddDependencyResolver(windsorDependencyResolver);
                var scanFoldersCommandProvider = _scanFoldersCommandProviderFactory.GetScanFoldersCommandProvider();
                var exitCode = scanFoldersCommandProvider.ScanFolders(uncPathsToScan, localDataFolder, uploadDataFolder, scanFoldersActorSystem);
                return exitCode;
            }
        }
    }
}
