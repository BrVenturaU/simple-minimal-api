using BackgroundDemo.Abstractions;
using System.Reflection;

namespace BackgroundDemo.Extensions;

public static class WebApplicationExtensions
{
    public static IApplicationBuilder MapEndpoints(
        this WebApplication app,
        RouteGroupBuilder? routeGroupBuilder = null)
    {
        var endpoints = app.Services.GetServices<IEndpoint>();

        IEndpointRouteBuilder builder =
            routeGroupBuilder is null ? app : routeGroupBuilder;

        foreach (IEndpoint endpoint in endpoints)
        {
            endpoint.MapEndpoint(builder);
        }

        return app;
    }

    public static IApplicationBuilder MapEndpoints(
        this WebApplication app,
        string mapFromNamespace,
        RouteGroupBuilder? routeGroupBuilder = null)
    {
        var endpoints = app
            .Services
            .GetServices<IEndpoint>()
            .Where(endpoint => endpoint.GetType().Namespace!.Contains(mapFromNamespace));
        IEndpointRouteBuilder builder =
            routeGroupBuilder is null ? app : routeGroupBuilder;

        foreach (IEndpoint endpoint in endpoints)
        {
            endpoint.MapEndpoint(builder);
        }

        return app;
    }
}
