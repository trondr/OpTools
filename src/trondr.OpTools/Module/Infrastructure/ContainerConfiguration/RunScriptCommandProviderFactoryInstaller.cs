using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using trondr.OpTools.Infrastructure.ContainerExtensions;
using trondr.OpTools.Library.Module.Commands.RunScript;

namespace trondr.OpTools.Module.Infrastructure.ContainerConfiguration
{
    [InstallerPriority(InstallerPriorityAttribute.DefaultPriority)]
    public class RunScriptCommandProviderFactoryInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            //Manual registrations
            container.Register(Component.For<IRunScriptCommandProviderFactory>().AsFactory());

            container.Register(Component.For<IRunScriptCommandProvider>().Named(typeof(RunScriptCommandProvider).Name).ImplementedBy<RunScriptCommandProvider>().LifestyleTransient());
        }
    }
}