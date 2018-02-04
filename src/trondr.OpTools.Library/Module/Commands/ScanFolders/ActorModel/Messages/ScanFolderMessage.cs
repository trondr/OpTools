using System;
using System.IO;
using Akka.Actor;
using LanguageExt;
using trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Actors;

namespace trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Messages
{
    internal class ScanFolderMessage : Record<ScanFolderMessage>
    {
        public string UncPath { get; }
        public string HostName { get; }
        public IActorRef UsageWriterActor { get; }
        public IActorRef ProcessFolderActorRouter { get; }
        public string Name { get; }

        private ScanFolderMessage(string uncPath, string hostName, IActorRef usageWriterActor,
            IActorRef processFolderActorRouter)
        {
            this.UncPath = uncPath;
            this.HostName = hostName;
            UsageWriterActor = usageWriterActor;
            ProcessFolderActorRouter = processFolderActorRouter;
            this.Name = GetNameFromUncPath(uncPath);
        }

        private string GetNameFromUncPath(string uncPath)
        {
            return uncPath
                .Replace(Path.DirectorySeparatorChar, '_')
                .Replace(Path.AltDirectorySeparatorChar, '_')
                .Replace(' ', '_');
        }

        public static Result<ScanFolderMessage> Create(string uncPath, IActorRef usageWriterActor,
            IActorRef processFolderActorRouter)
        {
            if (!Directory.Exists(uncPath))
                return new Result<ScanFolderMessage>(new DirectoryNotFoundException($"Unc path '{uncPath}' not found."));

            var uncPathUri = new Uri(uncPath);
            if(!uncPathUri.IsUnc)
                return new Result<ScanFolderMessage>(new ArgumentException($"Not an unc path: '{uncPath}'."));

            if (usageWriterActor == null)
            {
                return new Result<ScanFolderMessage>(new ArgumentNullException($"{typeof(UsageWriterActor).Name} actor ref is null."));
            }

            return new Result<ScanFolderMessage>(new ScanFolderMessage(uncPathUri.OriginalString,uncPathUri.Host, usageWriterActor, processFolderActorRouter));
        }
    }
}