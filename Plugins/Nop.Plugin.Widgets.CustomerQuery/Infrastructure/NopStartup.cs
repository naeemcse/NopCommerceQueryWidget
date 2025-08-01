using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Plugin.Widgets.CustomerQuery.Factories;
using Nop.Plugin.Widgets.CustomerQuery.Services;
using Nop.Services.Events;
using Nop.Web.Framework.Events;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.Widgets.CustomerQuery.Infrastructure;

/// <summary>
/// Represents object for the configuring services on application startup
/// </summary>
public class NopStartup : INopStartup
{
    /// <summary>
    /// Add and configure any of the middleware
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <param name="configuration">Configuration of the application</param>
    public virtual void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        // Register menu provider       
        services.AddScoped<IRouteProvider, RouteProvider>();
        services.AddScoped<ICustomerQueryService, CustomerQueryService>();
        services.AddScoped<IConsumer<AdminMenuCreatedEvent>, EventConsumer>();
        services.AddScoped<ICustomerQueryModelFactory, CustomerQueryModelFactory>();

    }

    /// <summary>
    /// Configure the using of added middleware
    /// </summary>
    /// <param name="application">Builder for configuring an application's request pipeline</param>
    public void Configure(IApplicationBuilder application)
    {
        // Add any middleware configuration if needed
    }

    /// <summary>
    /// Gets order of this startup configuration implementation
    /// </summary>
    public int Order => 3000;
}