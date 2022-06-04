using FastEndpoints.Example.Endpoints.Login;
using Microsoft.Net.Http.Headers;

namespace API.E2E.Tests;

public abstract class EndToEndTestCase
{
    protected readonly WebApplicationFactory<Program> Application;
    protected readonly HttpClient Client;
    protected readonly Fixture Fixture = new();
    protected EndToEndTestCase()
    {
        Application = new WebApplicationFactory<Program>();
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
            Email = "admin2@test.com",
            Password = "password"
        };

        var response = await Client.PostAsJsonAsync("/auth/login", role switch {
            "user" => userRequest,
            "admin" => adminRequest,
            _ => adminRequest
        });

        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadAsStringAsync();

        var cookie = response.Headers.GetValues(HeaderNames.SetCookie).First();

        //var cookie = response.ExtractCookie("Auth");

        Client.DefaultRequestHeaders.Add(HeaderNames.Cookie, cookie);
    }
}
