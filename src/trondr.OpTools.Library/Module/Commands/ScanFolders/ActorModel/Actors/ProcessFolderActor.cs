using System;
using System.Linq;
using System.Security.AccessControl;
using Akka.Actor;
using Akka.Event;
using Akka.Util.Internal;
using trondr.OpTools.Library.Infrastructure;
using trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Messages;
using trondr.OpTools.Library.Module.Common;
using Path = Pri.LongPath.Path;
using Directory = Pri.LongPath.Directory;
using DirectoryInfo = Pri.LongPath.DirectoryInfo;
using File = Pri.LongPath.File;
using FileSystemInfo = Pri.LongPath.FileSystemInfo;

namespace trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Actors
{
    public class ProcessFolderActor : ReceiveActor
    {
        private ILoggingAdapter _logger;
        private ILoggingAdapter Logger => _logger ?? (_logger = Context.GetLogger());

        public ProcessFolderActor()
        {
            Receive<ProcessFolderMessage>(message => OnProcessFolderMessage(message));
        }

        private void OnProcessFolderMessage(ProcessFolderMessage message)
        {
            try
            {
                var directory = new DirectoryInfo(message.UncPath);
                var subDirectories = directory.GetDirectories();
                subDirectories.ForEach(subDirectory =>
                {
                    var processFolderMessage =
                        ProcessFolderMessage.Create(subDirectory.FullName, message.ScanFoldersActors);
                    processFolderMessage.IfSucc(folderMessage =>
                    {
                        message.ScanFoldersActors.CoordinatorActor.Tell(folderMessage);
                    });
                    processFolderMessage.IfFail(exception =>
                    {
                        Logger.Error(
                            $"Failed to create {typeof(ProcessFolderMessage).Name} from path '{message.UncPath}' in {this.GetType().Namespace}.  {exception.Message}");
                    });
                });

                var files = directory.GetFiles();
                var size = files.Sum(file => file.Length);
                //ToDo.Implement(ToDoPriority.Critical, "trondr", "Implement get of sddl");
                var directorySecurity = directory.GetAccessControl(AccessControlSections.Access);
                var sddl = directorySecurity.GetAccessControlSddlForm(AccessControlSections.Access, true);
                var isProtected = directorySecurity.AreAccessRulesProtected;
                var comment = ""; //Any error message here, otherwise empty
                message.ScanFoldersActors.UsageWriterActor.Tell(new UsageRecordMessage(message.HostName,
                    size.ToString(), directory.FullName, sddl,isProtected.ToString(), comment));
                message.ScanFoldersActors.CoordinatorActor.Tell(new ProcessedFolderMessage());
            }
            catch (UnauthorizedAccessException ex)
            {
                message.ScanFoldersActors.UsageWriterActor.Tell(new UsageRecordMessage(message.HostName, 0.ToString(),
                    message.UncPath, "", "", ex.Message));
                message.ScanFoldersActors.CoordinatorActor.Tell(new ProcessedFolderMessage());
            }            
        }

        protected override void PostStop()
        {
            Logger.Info($"{GetType().Name}({Self.Path}) has stopped.");
            base.PostStop();
        }
    }
}