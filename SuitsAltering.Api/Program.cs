using System.Reflection;
using Autofac;
using Microsoft.EntityFrameworkCore;
using SuitsAltering.DAL;
using SuitsAltering.API.DependencyResolving;
using SuitsAltering.BL.DependencyResolving;
using SuitsAltering.DAL.DependencyResolving;
using Autofac.Extensions.DependencyInjection;
using SuitsAltering.BL.ServiceBus;
using SuitsAltering.BL.Models;
using SuitsAltering.API.Middleware;
using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.ApplicationInsights.Extensibility;
using Serilog;
using SuitsAltering.API.Validation;

var builder = WebApplication.CreateBuilder(args);


builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
{
    loggerConfiguration
        .WriteTo.Console()
        .WriteTo.ApplicationInsights(
            new TelemetryConfiguration { ConnectionString = hostingContext.Configuration["ApplicationInsights:ConnectionString"] },
            TelemetryConverter.Events);
});
builder.Services.AddApplicationInsightsTelemetry(new ApplicationInsightsServiceOptions(){ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"] });
builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddTransient<IValidatorInterceptor, ValidatorInterceptor>();
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(MapperBLProfiles).Assembly);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer((Action<ContainerBuilder>)(x =>
{

    var assemblies = new Dictionary<Type, Assembly?>
    {
        { typeof(IApiModule), Assembly.GetAssembly(typeof(IApiModule)) },
        { typeof(IBLModule), Assembly.GetAssembly(typeof(IBLModule)) },
        { typeof(IDalModule), Assembly.GetAssembly(typeof(IDalModule)) },
    };

    x.RegisterAssemblyModules<IApiModule>(assemblies[typeof(IApiModule)]);
    x.RegisterAssemblyModules<IBLModule>(assemblies[typeof(IBLModule)]);
    x.RegisterAssemblyModules<IDalModule>(assemblies[typeof(IDalModule)]);
}));

builder.Services.Configure<AzureServiceBusSettings>(builder.Configuration.GetSection(AzureServiceBusSettings.SectionName));

builder.Services.AddDbContext<SuitsDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration["DbConnectionString"]);
});
var app = builder.Build();
    
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
