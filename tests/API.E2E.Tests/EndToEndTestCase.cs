using FastEndpoints.Example.Endpoints.Auth.Login;
using FastEndpoints.Example.Endpoints.Users.CreateUserEndpoint;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace API.E2E.Tests;

public abstract class EndToEndTestCase : IAsyncDisposable
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

    protected async Task AuthenticateAsync(LoginRequest request)
    {
        var response = await Client.PostAsJsonAsync("/auth/login", request);

        response.EnsureSuccessStatusCode();

        var cookie = response.Headers.GetValues(HeaderNames.SetCookie).First();

        Client.DefaultRequestHeaders.Add(HeaderNames.Cookie, cookie);
    }

    protected async Task RegisterAsync(CreateUserRequest request)
    {
        var response = await Client.PostAsJsonAsync("/users", request);

        response.EnsureSuccessStatusCode();
    }

    public async ValueTask DisposeAsync()
    {
        var client = Application.Services.GetRequiredService<IMongoClient>();

        var iterator = await client.ListDatabaseNamesAsync();

        if (iterator is null)
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
    }
}
