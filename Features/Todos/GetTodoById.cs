using BackgroundDemo.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace BackgroundDemo.Features.Todos;

public class GetTodoById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGet("/{id:guid}",
            (Guid userId, Guid id, [FromServices] TodoStorage storage) =>
            {
                var todoList = storage.Todos.GetValueOrDefault(userId);

                var todo = todoList.FirstOrDefault(todo => todo.Id == id);
                if (todo is null)
                    return Results.NotFound("The TODO does not exists.");

                return Results.Ok(todo);
            }).WithName("GetTodoById");
    }
}
