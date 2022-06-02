using System.Text.Json;
using FastEndpoints;
using FastEndpoints.Example.Extensions;
using FastEndpoints.Example.Repositories;
using FastEndpoints.Example.RouteConstraints;
using FastEndpoints.Example.Services;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RouteOptions>(x =>
{
    x.ConstraintMap.Add("mongoId", typeof(MongoIdConstraint));
});

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IHasherService, HasherService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.Name = "Auth";
    options.ExpireTimeSpan = TimeSpan.FromHours(2);
    options.LoginPath = null;
    options.LogoutPath = null;
    options.AccessDeniedPath = null;
    options.Events = new CookieAuthenticationEvents
    {
        OnRedirectToLogin = ctx =>
        {
            ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();
builder.Services.AddFastEndpoints();
builder.Services.AddSwaggerDoc();
builder.Services.AddMongo();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseHttpLogging();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints(options =>
{
    options.SerializerOptions = x => x.PropertyNamingPolicy = null;
    options.ErrorResponseStatusCode = StatusCodes.Status422UnprocessableEntity;
    options.ErrorResponseBuilder = (failures, _) => failures.ToResponse();
});

app.UseOpenApi();
app.UseSwaggerUi3(x => x.ConfigureDefaults());

await app.RunAsync();

public partial class Program { }