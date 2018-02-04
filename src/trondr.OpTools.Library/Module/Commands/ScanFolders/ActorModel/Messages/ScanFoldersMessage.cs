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
        public int ExitCode { get; set; }
        public bool OverWrite { get; }
        public int DegreeOfParallelism { get; }

        private ScanFoldersMessage(string[] uncPathsToScan, string localDataFolder, string uploadDataFolder, bool overWrite, int degreeOfParallelism)
        {
            UncPathsToScan = uncPathsToScan;
            LocalDataFolder = localDataFolder;
            UploadDataFolder = uploadDataFolder;
            OverWrite = overWrite;
            DegreeOfParallelism = degreeOfParallelism;
            ExitCode = 0;
        }

        public static Result<ScanFoldersMessage> Create(string[] uncPathsToScan, string localDataFolder,
            string uploadDataFolder, bool overWrite, int degreeOfParallelism)
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

            if (degreeOfParallelism <= 0 || degreeOfParallelism > 255)
            {
                return new Result<ScanFoldersMessage>(new ArgumentException("Parameter degreeOfParallelism must be between 1 and 255."));
            }            
            return new Result<ScanFoldersMessage>(new ScanFoldersMessage(uncPathsToScan, localDataFolder, uploadDataFolder, overWrite, degreeOfParallelism));
        }
    }
}