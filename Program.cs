using BackgroundDemo;
using BackgroundDemo.Abstractions;
using BackgroundDemo.Dtos;
using BackgroundDemo.Extensions;
using BackgroundDemo.Features.Todos;
using BackgroundDemo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Reflection;

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
builder.Services.AddEndpoints(typeof(Program).Assembly);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors();

//app
//    .MapGroup("/users/{userId:guid}/todos")
//    .MapTodosGroup()
//    .AddEndpointFilter(async (context, next) =>
//    {
//        var value = Guid.Parse(context.HttpContext.GetRouteValue("userId")!.ToString()!);
//        var storage = context.HttpContext.RequestServices.GetRequiredService<TodoStorage>();
//        var userExists = storage.Todos.TryGetValue(value, out var todoList);
//        if (!userExists)
//            return Results.NotFound("The user does not exists.");
//        return await next(context);
//    });



app.MapGet("/messages", (SampleData data) => data.Data.Order());

RouteGroupBuilder userTodosGroup = app
    .MapGroup("/users/{userId:guid}/todos")
    .AddEndpointFilter(async (context, next) =>
    {
        var value = Guid.Parse(context.HttpContext.GetRouteValue("userId")!.ToString()!);
        var storage = context.HttpContext.RequestServices.GetRequiredService<TodoStorage>();
        var userExists = storage.Todos.TryGetValue(value, out var todoList);
        if (!userExists)
            return Results.NotFound("The user does not exists.");
        return await next(context);
    })
    .WithTags("Todos");

app.MapEndpointsFromNamespace(typeof(CreateTodo).Namespace!, userTodosGroup);
app.MapRemainingEndpoints();
app.Run();