using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using trondr.OpTools.Infrastructure.ContainerExtensions;
using trondr.OpTools.Library.Module.Commands.RunScript.ActorModel.Actors;
using trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Actors;

namespace trondr.OpTools.Module.Infrastructure.ContainerConfiguration
{
    [InstallerPriority(InstallerPriorityAttribute.DefaultPriority)]
    public class ScanFolderActorContainerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            //Manual registrations
            container.Register(Component.For<ScanFolderActor>().LifestyleTransient().Activator<StrictComponentActivator>());
        }
    }
}