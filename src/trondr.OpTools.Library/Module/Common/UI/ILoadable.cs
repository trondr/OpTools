using System.Threading.Tasks;

namespace trondr.OpTools.Library.Module.Common.UI
{
    public interface ILoadable
    {        
        Task LoadAsync();
        
        Task UnloadAsync();

        LoadStatus LoadStatus { get; set; }
    }
}