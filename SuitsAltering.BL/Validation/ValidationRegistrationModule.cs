using Autofac;
using SuitsAltering.BL.DependencyResolving;

namespace SuitsAltering.BL.Validation
{
    public class ValidationRegistrationModule : Module, IBLModule
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(BusinessValidationBuilder<>)).As(typeof(IBusinessValidationBuilder<>)).InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(typeof(IBusinessValidator<>).Assembly).AsClosedTypesOf(typeof(IBusinessValidator<>));
            builder.RegisterAssemblyTypes(typeof(IBusinessValidator<,>).Assembly).AsClosedTypesOf(typeof(IBusinessValidator<,>));
        }
    }
}
