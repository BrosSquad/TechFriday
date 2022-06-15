using MinimalAPI.Models;
using MongoDB.Driver;

namespace MinimalAPI.Extensions;

public static class AppExtensions
{
    public static void AddMongo(this IServiceCollection services)
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
    }
}
