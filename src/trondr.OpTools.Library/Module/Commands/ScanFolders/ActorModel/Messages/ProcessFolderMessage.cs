using Akka.Actor;

namespace trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Messages
{
    public class ProcessFolderMessage
    {
        public ProcessFolderMessage(string hostName, string uncPath, IActorRef usageWriterActor)
        {
            UncPath = uncPath;
            UsageWriterActor = usageWriterActor;
            HostName = hostName;
        }

        public string HostName { get; }
        public string UncPath { get; }
        public IActorRef UsageWriterActor { get; }
        
    }
}