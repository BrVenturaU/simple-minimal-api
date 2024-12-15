using BackgroundDemo.Abstractions;
using BackgroundDemo.Dtos;
using BackgroundDemo.Models;
using Microsoft.AspNetCore.Mvc;

namespace BackgroundDemo.Features.Todos;

public class CreateTodo : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapPost("/",
            (Guid userId, [FromBody] CreateUpdateTodoDto todoDto, [FromServices] TodoStorage storage) =>
            {
                var todoList = storage.Todos.GetValueOrDefault(userId);
                Todo todo = new(Guid.NewGuid(), todoDto.Description, todoDto.Finished);
                todoList?.Add(todo); // Relying on the collection reference.

                return Results.CreatedAtRoute("GetTodoById", new { userId, todo.Id }, todo);
            });
    }
}
