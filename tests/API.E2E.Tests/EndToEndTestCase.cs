using AutoFixture;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;
using System.Net;
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

    protected static async Task<(HttpStatusCode statusCode, T data)> Extract<T>(HttpResponseMessage response)
    {
        var stream = await response.Content.ReadAsStreamAsync();

        var body = await JsonSerializer.DeserializeAsync<T>(stream);

        return (response.StatusCode, body)!;
    }

    protected static string ExtractCookie(HttpResponseMessage response, string name)
    {
        var cookies = response.Headers.GetValues(HeaderNames.SetCookie);

        foreach(var cookie in cookies)
        {
            if (cookie.Contains(name))
            {
                return cookie;
            }
        }

        return string.Empty;
    }

}