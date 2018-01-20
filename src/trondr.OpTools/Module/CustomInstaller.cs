using System.Collections;
using System.ComponentModel;

namespace trondr.OpTools.Module
{
    [RunInstaller(true)]
    public partial class CustomInstaller : System.Configuration.Install.Installer
    {
        public CustomInstaller()
        {
            InitializeComponent();
        }

        public override void Install(IDictionary stateSaver)
        {
            //Example: Adding a command to windows explorer contect menu
            //this.Context.LogMessage("Adding trondr.OpTools to File Explorer context menu...");
            //new WindowsExplorerContextMenuInstaller().Install("trondr.OpTools", "Create Something...", Assembly.GetExecutingAssembly().Location, "CreateSomething /exampleParameter=\"%1\"");
            //this.Context.LogMessage("Finnished adding trondr.OpTools to File Explorer context menu.");
            
            base.Install(stateSaver);
        }

        public override void Uninstall(IDictionary savedState)
        {
            //Example: Removing previously installed command from windows explorer contect menu
            //this.Context.LogMessage("Removing trondr.OpTools from File Explorer context menu...");
            //new WindowsExplorerContextMenuInstaller().UnInstall("trondr.OpTools");
            //this.Context.LogMessage("Finished removing trondr.OpTools from File Explorer context menu.");
            
            base.Uninstall(savedState);
        }        
    }
}
