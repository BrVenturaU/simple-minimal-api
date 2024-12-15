using BackgroundDemo.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace BackgroundDemo.Features.Todos;

public class DeleteTodo : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapDelete("/{id:guid}",
            (Guid userId, Guid id, [FromServices] TodoStorage storage) =>
            {
                var todoList = storage.Todos.GetValueOrDefault(userId);

                var oldTodo = todoList.FirstOrDefault(todo => todo.Id == id);
                if (oldTodo is null)
                    return Results.NotFound("The TODO does not exists.");

                todoList.Remove(oldTodo);

                return Results.NoContent();
            });
    }
}
