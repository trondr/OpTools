using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using trondr.OpTools.Infrastructure.ContainerExtensions;
using trondr.OpTools.Library.Module.Commands.ScanFolders;

namespace trondr.OpTools.Module.Infrastructure.ContainerConfiguration
{
    [InstallerPriority(InstallerPriorityAttribute.DefaultPriority)]
    public class ScanFoldersCommandProviderFactoryInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            //Manual registrations
            container.Register(Component.For<IScanFoldersCommandProviderFactory>().AsFactory());

            container.Register(Component.For<IScanFoldersCommandProvider>().Named(typeof(ScanFoldersCommandProvider).Name).ImplementedBy<ScanFoldersCommandProvider>().LifestyleTransient());
        }
    }
}