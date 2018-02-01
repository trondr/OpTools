namespace trondr.OpTools.Library.Module.Commands.ScanFolders
{
    public interface IScanFoldersCommandProviderFactory
    {
        IScanFoldersCommandProvider GetScanFoldersCommandProvider();

        void Release(IScanFoldersCommandProvider scanFoldersCommandProvider);
    }
}
