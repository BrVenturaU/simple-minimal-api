namespace BackgroundDemo.Abstractions;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder routeBuilder);
}
