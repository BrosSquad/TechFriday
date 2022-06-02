using FastEndpoints.Example.Models;
using FastEndpoints.Example.Options;
using FluentValidation.Results;
using Microsoft.Extensions.Options;
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
            var mongo = x.GetRequiredService<IOptions<MongoOptions>>().Value;

            return new MongoClient(mongo.ConnectionString);
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
