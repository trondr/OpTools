using System.Linq;
using Akka.Actor;
using Akka.Util.Internal;
using trondr.OpTools.Library.Infrastructure;
using trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Messages;
using DirectoryInfo = Pri.LongPath.DirectoryInfo;

namespace trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Actors
{
    public class ProcessFolderActor : ReceiveActor
    {
        public ProcessFolderActor()
        {
            Receive<ProcessFolderMessage>(message => OnProcessFolderMessage(message));
        }

        private void OnProcessFolderMessage(ProcessFolderMessage message)
        {
            var directory = new DirectoryInfo(message.UncPath);

            var subDirectories = directory.GetDirectories();
            subDirectories.ForEach(info => Context.Parent.Tell(new ProcessFolderMessage(message.HostName,info.FullName,message.UsageWriterActor)));

            var files = directory.GetFiles();
            var size = files.Sum(file => file.Length);
            ToDo.Implement(ToDoPriority.Critical,"trondr","Implement get of sddl");
            var sddl = "";
            var comment = ""; //Any error message here, otherwise empty
            message.UsageWriterActor.Tell(new UsageRecordMessage(message.HostName, size.ToString(), directory.FullName, sddl,comment));
        }
    }
}