using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using trondr.OpTools.Infrastructure.ContainerExtensions;
using trondr.OpTools.Library.Module.Commands.RunScript.ActorModel;
using trondr.OpTools.Library.Module.Commands.RunScript.ActorModel.Actors;

namespace trondr.OpTools.Module.Infrastructure.ContainerConfiguration
{
    [InstallerPriority(InstallerPriorityAttribute.DefaultPriority)]
    public class OnlineStatusActorContainerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            //Manual registrations
            container.Register(Component.For<OnlineStatusActor>().LifestyleTransient().Activator<StrictComponentActivator>());            
        }
    }
}
