using System.Security.Claims;
using FastEndpoints;
using FastEndpoints.Example.Extensions;
using FastEndpoints.Example.RouteConstraints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RouteOptions>(x =>
{
    x.ConstraintMap.Add("mongoId", typeof(MongoIdConstraint));
});

builder.WebHost.ConfigureKestrel(x => x.AddServerHeader = false);
builder.Services.AddServicesRepositories();

builder.Services.AddOptionModels();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
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
        },
        OnRedirectToAccessDenied = async ctx =>
        {
            ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await ctx.Response.WriteAsJsonAsync(new { Message = "Unauthorized." });
        }
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", x => x.RequireClaim(ClaimTypes.Role, "Admin"));
    options.AddPolicy("UserOnly", x => x.RequireClaim(ClaimTypes.Role, "User"));
});

builder.Services.AddFastEndpoints(options =>
{
    options.SourceGeneratorDiscoveredTypes = DiscoveredTypes.All;
});
builder.Services.AddSwaggerDoc(addJWTBearerAuth: false);
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
    options.SerializerOptions = x => x.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    options.ErrorResponseStatusCode = StatusCodes.Status422UnprocessableEntity;
    options.ErrorResponseBuilder = (failures, _) => failures.ToResponse();
});

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi3(x => x.ConfigureDefaults());
}

await app.RunAsync();

public partial class Program { }
