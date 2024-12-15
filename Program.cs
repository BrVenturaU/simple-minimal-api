using BackgroundDemo;
using BackgroundDemo.Dtos;
using BackgroundDemo.Extensions;
using BackgroundDemo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(config =>
{
    config.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<SampleData>();
builder.Services.AddSingleton<TodoStorage>();
builder.Services.AddHostedService<BackgroundRefresher>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors();
app
    .MapGroup("/users/{userId:guid}/todos")
    .MapTodosGroup()
    .AddEndpointFilter(async (context, next) =>
    {
        var value = Guid.Parse(context.HttpContext.GetRouteValue("userId")!.ToString()!);
        var storage = context.HttpContext.RequestServices.GetRequiredService<TodoStorage>();
        var userExists = storage.Todos.TryGetValue(value, out var todoList);
        if (!userExists)
            return Results.NotFound("The user does not exists.");
        return await next(context);
    });

app.MapPost("/users", ([FromServices] TodoStorage storage) =>
{
    var todoList = new List<Todo>();
    var user = Guid.NewGuid();
    var wasAdded = storage.Todos.TryAdd(user, todoList);
    if (!wasAdded)
        return Results.StatusCode((int)HttpStatusCode.InternalServerError);

    return Results.Ok(user);
});
app.MapGet("/messages", (SampleData data) => data.Data.Order());



app.Run();