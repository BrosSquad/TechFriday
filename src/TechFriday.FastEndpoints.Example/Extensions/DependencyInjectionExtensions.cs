using FastEndpoints.Example.Models;
using FastEndpoints.Example.Options;
using FastEndpoints.Example.Services;
using FastEndpoints.Example.Repositories;
using FluentValidation.Results;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace FastEndpoints.Example.Extensions;

public class ErrorResponse
{
    public string Property { get; init; } = default!;
    public string Message { get; init; } = default!;
}

public static class DependencyInjectionExtensions
{
    public static List<ErrorResponse> ToResponse(this IEnumerable<ValidationFailure> errors)
    {
        var list = new List<ErrorResponse>();

        foreach (var err in errors)
        {

            list.Add(new ErrorResponse
            {
                Property = err.PropertyName,
                Message = err.ErrorMessage
            });
        }

        return list;
    }

    public static IServiceCollection AddMongo(this IServiceCollection services)
    {
        services.AddSingleton<IMongoClient>(x =>
        {
            var mongo = x.GetRequiredService<IOptions<MongoOptions>>().Value;

            return new MongoClient(mongo.ConnectionString);
        });

        services.AddSingleton(x =>
        {
            var mongo = x.GetRequiredService<IOptions<MongoOptions>>().Value;

            return x.GetRequiredService<IMongoClient>().GetDatabase(mongo.AppName);
        });

        services.AddSingleton(x =>
            x.GetRequiredService<IMongoDatabase>()
                .GetCollection<User>(nameof(User))
        );

        return services;
    }

    public static IServiceCollection AddServicesRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ILoginService, LoginService>();
        services.AddSingleton<IUserRepository, UserRepository>();
        services.AddSingleton<IHasherService, HasherService>();
        services.AddScoped<ILoginServiceExt, LoginServiceExt>();

        return services;
    }

    public static IServiceCollection AddOptionModels(this IServiceCollection services)
    {
        services.AddOptions<MongoOptions>()
            .BindConfiguration(MongoOptions.Section)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }
}