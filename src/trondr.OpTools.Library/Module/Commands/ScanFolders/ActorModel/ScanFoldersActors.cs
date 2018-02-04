using System;
using Akka.Actor;

namespace trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel
{
    public class ScanFoldersActors
    {
        public ScanFoldersActors(IActorRef coordinatorActor, IActorRef usageWriterActor)
        {
            CoordinatorActor = coordinatorActor ?? throw new ArgumentNullException(nameof(coordinatorActor));
            UsageWriterActor = usageWriterActor ?? throw new ArgumentNullException(nameof(usageWriterActor));
        }

        public IActorRef UsageWriterActor { get; }
        
        public IActorRef CoordinatorActor { get; }
    }
}