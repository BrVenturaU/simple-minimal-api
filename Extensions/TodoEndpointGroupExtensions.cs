using BackgroundDemo.Dtos;
using BackgroundDemo.Models;
using Microsoft.AspNetCore.Mvc;

namespace BackgroundDemo.Extensions;

public static class TodoEndpointGroupExtensions
{
    public static RouteGroupBuilder MapTodosGroup(this RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapGet("/", (Guid userId, [FromServices] TodoStorage storage) =>
        {
            var todoList = storage.Todos.GetValueOrDefault(userId);

            return Results.Ok(todoList);
        });

        groupBuilder.MapGet("/{id:guid}",
            (Guid userId, Guid id, [FromServices] TodoStorage storage) =>
            {
                var todoList = storage.Todos.GetValueOrDefault(userId);

                var todo = todoList.FirstOrDefault(todo => todo.Id == id);
                if (todo is null)
                    return Results.NotFound("The TODO does not exists.");

                return Results.Ok(todo);
            }).WithName("GetTodoById");

        groupBuilder.MapPost("/",
            (Guid userId, [FromBody]CreateUpdateTodoDto todoDto, [FromServices] TodoStorage storage) =>
            {
                var todoList = storage.Todos.GetValueOrDefault(userId);
                Todo todo = new(Guid.NewGuid(), todoDto.Description, todoDto.Finished);
                todoList?.Add(todo); // Relying on the collection reference.

                return Results.CreatedAtRoute("GetTodoById", new { userId, todo.Id }, todo);
            });

        groupBuilder.MapPut("/{id:guid}",
            (Guid userId, Guid id, [FromBody]CreateUpdateTodoDto todoDto, [FromServices] TodoStorage storage) =>
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


        groupBuilder.MapDelete("/{id:guid}",
            (Guid userId, Guid id, [FromServices] TodoStorage storage) =>
            {
                var todoList = storage.Todos.GetValueOrDefault(userId);

                var oldTodo = todoList.FirstOrDefault(todo => todo.Id == id);
                if (oldTodo is null)
                    return Results.NotFound("The TODO does not exists.");

                todoList.Remove(oldTodo);

                return Results.NoContent();
            });
        return groupBuilder;
    }
}
