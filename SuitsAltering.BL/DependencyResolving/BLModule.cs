using Autofac;
using Autofac.Core;
using Microsoft.Extensions.Configuration;
using SuitsAltering.BL.ServiceBus;
using SuitsAltering.BL.Services;
using SuitsAltering.Infrastructure;

namespace SuitsAltering.BL.DependencyResolving
{
    public interface IBLModule : IModule
    {
    }

    public class BLModule : Module, IBLModule
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AlteringService>().As<IAlteringService>().InstancePerLifetimeScope();
            builder.RegisterType<ServiceBusMessageSender>().As<IServiceBusMessageSender>().InstancePerLifetimeScope();
            builder.RegisterType<AzureServiceBusProvider>().As<IServiceBusInitializer>().InstancePerLifetimeScope();
            builder.RegisterBuildCallback(c =>
            {
                AsyncHelper.RunSync(() => c.Resolve<IServiceBusInitializer>().InitializeAsync(ServiceBusMessageSender.Topics));
            });
        }
    }
}
