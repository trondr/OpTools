using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using GalaSoft.MvvmLight.Messaging;
using trondr.OpTools.Infrastructure.ContainerExtensions;

namespace trondr.OpTools.Infrastructure.ContainerConfiguration
{
    [InstallerPriority(InstallerPriorityAttribute.DefaultPriority)]
    public class MessengerContainerInstallerDefault: IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IMessenger>().Instance(Messenger.Default).Activator<StrictComponentActivator>());
        }
    }
}
