using Autofac;
using Autofac.Core;
using Microsoft.ApplicationInsights.Extensibility;
using SuitsAltering.API.Common;
using SuitsAltering.BL.ServiceBus;

namespace SuitsAltering.API.DependencyResolving;

public interface IApiModule : IModule
{
}

public class ApiModule : Module, IApiModule
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<TelemetryInitializer>().As<ITelemetryInitializer>().SingleInstance();
        builder.RegisterType<AzureServiceBusProvider>().As<IServiceBusProvider>().InstancePerLifetimeScope();
        
    }
}