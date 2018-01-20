using System.Windows.Input;
using trondr.OpTools.Library.Module.Common.UI;

namespace trondr.OpTools.Library.Module.ViewModels
{
    public interface IMainViewModel: ILoadable
    {
        int MaxLabelWidth { get; set; }
        string ProductDescription { get; set; }
        string ProductDescriptionLabelText { get; set; }
        ICommand ExitCommand { get; set; }
        ICommand LoadCommand { get; set; }
        ICommand UnLoadCommand { get; set; }
    }
}
