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

        public int ScanFolders(string[] uncPathsToScan, string localDataFolder, string uploadDataFolder, bool overWrite, int degreeOfParallelism, ActorSystem scanFoldersActorSystem)
        {
            var exitCode = 0;

            _logger.Info("Starting scan folders actor coordinator");
            var scanFoldersCoordinatorActor = scanFoldersActorSystem.ActorOf(scanFoldersActorSystem.DI().Props<ScanFoldersCoordinatorActor>(), "ScanFoldersCoordinatorActor");
            
            _logger.Info("Parsing input parameters...");
            var scanFoldersMessageResult = ScanFoldersMessage.Create(uncPathsToScan, localDataFolder, uploadDataFolder, overWrite, degreeOfParallelism);

            scanFoldersMessageResult.IfSucc(scanFoldersMessage =>
            {
                _logger.Info($"Tell {typeof(ScanFoldersCoordinatorActor).Name} to start processing. Message: {scanFoldersMessage}...");
                scanFoldersCoordinatorActor.Tell(scanFoldersMessage);
                _logger.Info("Waiting for scan folders actor system to terminate...");
                scanFoldersActorSystem.WhenTerminated.Wait();
                _logger.Info($"{scanFoldersActorSystem.Name} has terminated!");
                exitCode = scanFoldersMessage.ExitCode;
            });

            scanFoldersMessageResult.IfFail(exception =>
            {
                _logger.Error($"Invalid input parameter. {exception.Message} Terminating.");                
                scanFoldersActorSystem.Terminate();
                _logger.Info("Waiting for scan folders actor system to terminate...");
                scanFoldersActorSystem.WhenTerminated.Wait();
                _logger.Info($"{scanFoldersActorSystem.Name} has terminated!");
                exitCode = 1;
            });
            return exitCode;
        }
    }
}