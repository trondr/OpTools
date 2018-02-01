namespace trondr.OpTools.Library.Module.Commands.RunScript
{
    public interface IRunScriptCommandProviderFactory
    {
        IRunScriptCommandProvider GetRunScriptCommandProvider();

        void Release(IRunScriptCommandProvider runScriptCommandProvider);
    }
}