using System;
using Akka.Actor;

namespace trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel
{
    public class ScanFoldersActors
    {
        public ScanFoldersActors(IActorRef coordinatorActor, IActorRef processFolderActorRouter, IActorRef usageWriterActor)
        {
            CoordinatorActor = coordinatorActor ?? throw new ArgumentNullException(nameof(coordinatorActor));
            ProcessFolderActorRouter = processFolderActorRouter ?? throw new ArgumentNullException(nameof(processFolderActorRouter));
            UsageWriterActor = usageWriterActor ?? throw new ArgumentNullException(nameof(usageWriterActor));
        }

        public IActorRef UsageWriterActor { get; }

        private IActorRef ProcessFolderActorRouter { get; }

        public IActorRef CoordinatorActor { get; }
    }
}