using System.Reflection;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MinimalAPI.Configurations;
using MinimalAPI.Extensions;
using MinimalAPI.Repositories;
using MinimalAPI.Requests;
using MinimalAPI.RouteConstraints;
using MinimalAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<MongoDbOptions>()
    .Bind(builder.Configuration.GetSection(MongoDbOptions.Section))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.Configure<RouteOptions>(options =>
{
    options.ConstraintMap.Add("mongoId", typeof(MongoIdConstraint));
});

builder.Services.AddHealthChecks();
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddMongo();

builder.Services.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(Program)));

var app = builder.Build();

app.UseHealthChecks("/health");

app.MapGet("/",
    () => new { Message = "Welcome To Tech Friday!", Hosts = new[] { "Dusan Malusev", "Stefan Bogdanovic" } });

app.MapGet("/users", async (IUserService userService) =>
{
    return await userService.GetUsersAsync();
}).WithName("users.index");

app.MapGet("/users/{id:mongoId}", async (string id, IUserService userService) =>
{
    var user = await userService.GetUserAsync(id);

    return user is null
        ? Results.NotFound(new { Message = "User not found." })
        : Results.Ok(user);

}).WithName("users.find");

app.MapPost("/users",
    async ([FromBody] CreateUserRequest request,
        IValidator<CreateUserRequest> validator,
        IUserService userService) =>
    {
        var errors = await validator.ValidateAsync(request);

        if (!errors.IsValid) return Results.UnprocessableEntity(errors.ToResponse());

        var user = await userService.CreateUserAsync(request);

        return Results.CreatedAtRoute("users.store", new { user.Id }, user);
    }).WithName("users.store");

app.MapPut("/users/{id:mongoId}", async (
    string id,
    IUserService userService,
    CreateUserRequest request,
    IValidator<CreateUserRequest> validator) =>

{
    var errors = await validator.ValidateAsync(request);

    if (!errors.IsValid) return Results.UnprocessableEntity(errors.ToResponse());

    await userService.UpdateUserAsync(id, request);

    return Results.NoContent();
}).WithName("users.update");

app.MapDelete("/users/{id:mongoId}", async (
    string id, IUserService userService) =>
{
    await userService.DeleteUserAsync(id);

    return Results.NoContent();
}).WithName("users.destroy");

await app.RunAsync();
