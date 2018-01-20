using System.Windows.Input;
using GalaSoft.MvvmLight;
using trondr.OpTools.Library.Module.Common.UI;

namespace trondr.OpTools.Library.Module.ViewModels
{
    public interface IMainWindowsViewModel: ILoadable
    {
        ViewModelBase SelectedViewModel { get; set; }

        ICommand LoadCommand { get; set; }

        ICommand UnLoadCommand { get; set; }
    }
}