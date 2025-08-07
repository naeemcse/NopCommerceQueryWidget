using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.Widgets.CustomerQuery.Infrastructure;

public class RouteProvider : IRouteProvider
{
    /// <summary>
    /// Register routes
    /// </summary>
    /// <param name="endpointRouteBuilder">Route builder</param>
    public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
    {     
        endpointRouteBuilder.MapControllerRoute(
               name: "Plugin.CustomerQuery",
               pattern: "customer-query",
               defaults: new { controller = "CustomerQuery", action = "Index" }
           );       
        endpointRouteBuilder.MapControllerRoute(
            name: "CustomerQuerySuccess",
            pattern: "customer-query/success",
            defaults: new { controller = "CustomerQuery", action = "Success" }
        );
        endpointRouteBuilder.MapControllerRoute(
            name: "CustomerMyQueryList",
            pattern: "my-query",
            defaults: new { controller = "CustomerQuery", action = "MyQuires" }
        );


    }

    /// <summary>
    /// Gets a priority of route provider
    /// </summary>
    public int Priority => 0;
}