using System;
using System.IO;
using System.Linq;
using LanguageExt;

namespace trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Messages
{
    public class ScanFoldersMessage: Record<ScanFoldersMessage>
    {
        public string[] UncPathsToScan { get; }
        public string LocalDataFolder { get; }
        public string UploadDataFolder { get; }

        private ScanFoldersMessage(string[] uncPathsToScan, string localDataFolder, string uploadDataFolder)
        {
            UncPathsToScan = uncPathsToScan;
            LocalDataFolder = localDataFolder;
            UploadDataFolder = uploadDataFolder;
        }

        public static Result<ScanFoldersMessage> Create(string[] uncPathsToScan, string localDataFolder,
            string uploadDataFolder)
        {
            if (!Directory.Exists(localDataFolder))
                return new Result<ScanFoldersMessage>(new DirectoryNotFoundException($"Local data folder '{localDataFolder}' not found."));

            if (!Directory.Exists(uploadDataFolder))
                return new Result<ScanFoldersMessage>(new DirectoryNotFoundException($"Upload data folder '{uploadDataFolder}' not found."));

            if (uncPathsToScan == null || uncPathsToScan.Length == 0)
                return new Result<ScanFoldersMessage>(new ArgumentNullException($"Parameter UncPathsToScan input array is empty or null."));

            var uncPathsToScanNotExisting = uncPathsToScan.Where(s => !Directory.Exists(s)).ToList();
            if (uncPathsToScanNotExisting.Count > 0)
            {
                return new Result<ScanFoldersMessage>(new DirectoryNotFoundException($"UncPathsToScan not existing: {string.Join(", ",uncPathsToScanNotExisting)}"));
            }
            return new Result<ScanFoldersMessage>(new ScanFoldersMessage(uncPathsToScan, localDataFolder, uploadDataFolder));
        }
    }
}