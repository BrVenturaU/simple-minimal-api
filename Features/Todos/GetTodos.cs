using BackgroundDemo.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace BackgroundDemo.Features.Todos;

public class GetTodos : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGet("/", (Guid userId, [FromServices] TodoStorage storage) =>
        {
            var todoList = storage.Todos.GetValueOrDefault(userId);

            return Results.Ok(todoList);
        });
    }
}
