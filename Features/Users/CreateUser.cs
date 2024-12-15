using BackgroundDemo.Abstractions;
using BackgroundDemo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;

namespace BackgroundDemo.Features.Users;

public class CreateUser : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapPost("/users", ([FromServices] TodoStorage storage) =>
        {
            var todoList = new List<Todo>();
            var user = Guid.NewGuid();
            var wasAdded = storage.Todos.TryAdd(user, todoList);
            if (!wasAdded)
                return Results.StatusCode((int)HttpStatusCode.InternalServerError);

            return Results.Ok(user);
        });
    }
}
