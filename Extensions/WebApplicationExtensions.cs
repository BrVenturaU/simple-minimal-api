using BackgroundDemo.Abstractions;
using System.Collections.Concurrent;
using System.Reflection;

namespace BackgroundDemo.Extensions;

public static class WebApplicationExtensions
{
    private static readonly HashSet<string> _namespaces = new HashSet<string>();
    public static IApplicationBuilder MapRemainingEndpoints(
        this WebApplication app,
        RouteGroupBuilder? routeGroupBuilder = null)
    {
        var endpoints = app
            .Services
            .GetServices<IEndpoint>()
            .Where(endpoint => !_namespaces.Contains(endpoint.GetType().Namespace!))
            .ToList();

        IEndpointRouteBuilder builder =
            routeGroupBuilder is null ? app : routeGroupBuilder;

        foreach (IEndpoint endpoint in endpoints)
        {
            _namespaces.Add(endpoint.GetType().Namespace!);
            endpoint.MapEndpoint(builder);
        }

        return app;
    }

    public static IApplicationBuilder MapEndpointsFromNamespace(
        this WebApplication app,
        string mapFromNamespace,
        RouteGroupBuilder? routeGroupBuilder = null)
    {

        var endpoints = app
            .Services
            .GetServices<IEndpoint>()
            .Where(endpoint => endpoint.GetType().Namespace!.Contains(mapFromNamespace) &&
                !_namespaces.Contains(endpoint.GetType().Namespace!));

        IEndpointRouteBuilder builder =
            routeGroupBuilder is null ? app : routeGroupBuilder;

        foreach (IEndpoint endpoint in endpoints)
        {
            endpoint.MapEndpoint(builder);
        }

        _namespaces.Add(mapFromNamespace);
        return app;
    }
}
