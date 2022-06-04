using FastEndpoints.Example.Endpoints.Login;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace API.E2E.Tests;

public abstract class EndToEndTestCase : IDisposable, IAsyncDisposable
{
    protected readonly WebApplicationFactory<Program> Application;
    protected readonly HttpClient Client;
    protected readonly Fixture Fixture = new();
    protected readonly string DatabaseName;

    protected EndToEndTestCase()
    {
        var random = new Random();

        DatabaseName = $"{DatabaseName}_{random.Next()}";

        Application = new WebApplicationFactory<Program>();

        Application = Application.WithWebHostBuilder(hostBuilder =>
        {
            hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton(provider =>
                    provider.GetRequiredService<IMongoClient>().GetDatabase(DatabaseName)
                );
            });
        });


        Client = Application.CreateClient();
    }

    protected async Task AuthenticateAsync(string role)
    {
        var userRequest = new LoginRequest
        {
            Email = "user@test.com",
            Password = "password"
        };

        var adminRequest = new LoginRequest
        {
            Email = "admin@test.com",
            Password = "password"
        };

        var response = await Client.PostAsJsonAsync("/auth/login", role switch
        {
            "user" => userRequest,
            "admin" => adminRequest,
            _ => adminRequest
        });

        response.EnsureSuccessStatusCode();

        var cookie = response.Headers.GetValues(HeaderNames.SetCookie).First();

        Client.DefaultRequestHeaders.Add(HeaderNames.Cookie, cookie);
    }

    public async ValueTask DisposeAsync()
    {
        var client = Application.Services.GetRequiredService<IMongoClient>();

        var iterator = await client.ListDatabaseNamesAsync();

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
                    await client.DropDatabaseAsync(database, token);
                }
            });

        await Application.DisposeAsync();

        Client.Dispose();
    }

    public void Dispose()
    {
        var client = Application.Services.GetRequiredService<IMongoClient>();

        var iterator = client.ListDatabaseNames();

        if (iterator == null)
        {
            return;
        }

        foreach (var db in iterator.ToList().Where(db => db != null && DatabaseName == db))
        {
            client.DropDatabase(db);
            return;
        }

        Application.Dispose();

        Client.Dispose();
    }
}
