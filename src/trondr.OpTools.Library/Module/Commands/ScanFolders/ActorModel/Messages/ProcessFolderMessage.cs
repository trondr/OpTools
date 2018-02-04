using System;
using System.IO;
using Akka.Actor;
using LanguageExt;
using trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Actors;
using Path = Pri.LongPath.Path;
using Directory = Pri.LongPath.Directory;
using DirectoryInfo = Pri.LongPath.DirectoryInfo;
using File = Pri.LongPath.File;
using FileSystemInfo = Pri.LongPath.FileSystemInfo;


namespace trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Messages
{
    public class ProcessFolderMessage
    {
        public string HostName { get; }
        public string UncPath { get; }
        public ScanFoldersActors ScanFoldersActors { get; }
        
        private ProcessFolderMessage(string uncPath, string hostName, ScanFoldersActors scanFoldersActors)
        {
            UncPath = uncPath;
            HostName = hostName;
            ScanFoldersActors = scanFoldersActors;
        }

        public static Result<ProcessFolderMessage> Create(string uncPath, ScanFoldersActors scanFoldersActors)
        {
            if (!Directory.Exists(uncPath))
                return new Result<ProcessFolderMessage>(new DirectoryNotFoundException($"Unc path '{uncPath}' not found."));

            var uncPathUri = new Uri(uncPath);
            if (!uncPathUri.IsUnc)
                return new Result<ProcessFolderMessage>(new ArgumentException($"Not an unc path: '{uncPath}'."));

            if (scanFoldersActors == null)
            {
                return new Result<ProcessFolderMessage>(new ArgumentNullException($"{typeof(ScanFoldersActors).Name} is null."));
            }

            return new Result<ProcessFolderMessage>(new ProcessFolderMessage(uncPathUri.OriginalString, uncPathUri.Host, scanFoldersActors));
        }
    }
}