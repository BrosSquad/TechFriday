using Microsoft.Extensions.Configuration;

namespace Repository.Integration.Tests;

public class IntegrationTestCase : IDisposable, IAsyncDisposable
{
    protected IMongoClient Client;
    protected IMongoDatabase Database;

    protected string DatabaseName;

    public IntegrationTestCase()
    {
        var random = new Random();

        var config = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddJsonFile("appsettings.Testing.json")
            .Build();

        Client = new MongoClient(MongoClientSettings.FromConnectionString(
            config.GetConnectionString("MongoDBTesting") ?? "mongodb://localhost:27017"
        ));

        var databaseName = config.GetValue<string>("MongoSettings:Database") ?? "demo_test";

        DatabaseName = $"{databaseName}_{random.Next()}";

        Database = Client.GetDatabase(DatabaseName);
    }

    public void Dispose()
    {
        var iterator = Client.ListDatabaseNames();

        if (iterator == null)
        {
            return;
        }


        foreach (var db in iterator.ToList().Where(db => db != null && DatabaseName == db))
        {
            Client.DropDatabase(db);
            return;
        }
    }

    public async ValueTask DisposeAsync()
    {
        var iterator = await Client.ListDatabaseNamesAsync();

        if (iterator == null)
        {
            return;
        }

        var databases = await iterator.ToListAsync();

        await Parallel.ForEachAsync(databases.Where(db => db != null && DatabaseName == db),
            async (database, token) =>
            {
                if (!token.IsCancellationRequested)
                {
                    await Client.DropDatabaseAsync(database, token);
                }
            });
    }
}