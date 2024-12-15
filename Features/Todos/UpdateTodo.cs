using BackgroundDemo.Abstractions;
using BackgroundDemo.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BackgroundDemo.Features.Todos;

public class UpdateTodo : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapPut("/{id:guid}",
            (Guid userId, Guid id, [FromBody] CreateUpdateTodoDto todoDto, [FromServices] TodoStorage storage) =>
            {
                var todoList = storage.Todos.GetValueOrDefault(userId);

                var oldTodo = todoList.FirstOrDefault(todo => todo.Id == id);
                if (oldTodo is null)
                    return Results.NotFound("The TODO does not exists.");

                var todoIndex = todoList.IndexOf(oldTodo);
                var newTodo = oldTodo with
                {
                    Description = todoDto.Description,
                    Finished = todoDto.Finished,
                };

                todoList[todoIndex] = newTodo;

                return Results.NoContent();
            });
    }
}
