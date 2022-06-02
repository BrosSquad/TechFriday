using FastEndpoints.Example.Models;
using FluentValidation.Results;
using MongoDB.Driver;

namespace FastEndpoints.Example.Extensions;

public class ErrorResponse
{
    public string Property {get; init;} = default!;
    public string Message {get; init;} = default!;
}

public static class ValidationExtensions
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
            return new MongoClient(
                "mongodb://localhost:27017/?connectTimeoutMS=10&maxPoolSize=200&minPoolSize=4&maxIdleTimeMS=300&appName=testDb"
            );
        });

        services.AddSingleton(
            x => x.GetRequiredService<IMongoClient>().GetDatabase("testDb")
        );

        services.AddSingleton(x =>
            x.GetRequiredService<IMongoDatabase>()
                .GetCollection<User>(nameof(User))
        );

        return services;
    }
}
