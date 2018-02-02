using Akka.Actor;
using Akka.DI.Core;
using Common.Logging;
using trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Actors;
using trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Messages;

namespace trondr.OpTools.Library.Module.Commands.ScanFolders
{
    public class ScanFoldersCommandProvider : IScanFoldersCommandProvider
    {
        private readonly ILog _logger;

        public ScanFoldersCommandProvider(ILog logger)
        {
            _logger = logger;
        }

        public int ScanFolders(string[] uncPathsToScan, string localDataFolder, string uploadDataFolder,
            ActorSystem scanFoldersActorSystem)
        {
            var exitCode = 0;

            _logger.Info("Starting scan folders coordinator");
            var scanFoldersCoordinatorActor = scanFoldersActorSystem.ActorOf(scanFoldersActorSystem.DI().Props<ScanFoldersCoordinatorActor>(), "ScanFoldersCoordinatorActor");

            _logger.Info("Parsing input parameters...");
            var scanFoldersMessageResult = ScanFoldersMessage.Create(uncPathsToScan, localDataFolder, uploadDataFolder);

            scanFoldersMessageResult.IfSucc(scanFoldersMessage =>
            {
                _logger.Info($"Tell run script coordinator to start processing. Message: {scanFoldersMessage}...");
                scanFoldersCoordinatorActor.Tell(scanFoldersMessage);
            });

            scanFoldersMessageResult.IfFail(exception =>
            {
                _logger.Error($"Invalid input parameter. {exception.Message} Terminating.");                
                scanFoldersActorSystem.Terminate();
                exitCode = 1;
            });

            _logger.Info("Waiting for scan folders actor system to terminate...");
            scanFoldersActorSystem.WhenTerminated.Wait();
            _logger.Info("Actor system has terminated!");
            return exitCode;
        }
    }
}