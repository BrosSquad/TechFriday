using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using AutoFixture;
using FastEndpoints.Example.Endpoints;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace MinimalAPI.Integration.Tests;

public class IntegrationTest
{
    protected readonly WebApplicationFactory<Program> Application;
    protected readonly HttpClient Client;
    protected readonly Fixture Fixture = new();

    public IntegrationTest()
    {
        Application = new WebApplicationFactory<Program>();
        Client = Application.CreateClient();
    }

    [Fact]
    public async Task Test()
    {
        // Arrange
        const string url = "/";

        // Act
        var (status, response) = await Extract<Response>(
            await Client.GetAsync(url)
        );

        // Assert
        response.Should().NotBeNull();
        response.Should().BeOfType<Response>();
        response!.Uptime.Should().NotBeNull();
        status.Should().Be(HttpStatusCode.OK);
    }

    protected static async Task<(HttpStatusCode statusCode, T data)> Extract<T>(HttpResponseMessage response) where T : class
    {
        var stream = await response.Content.ReadAsStreamAsync();

        var body = await JsonSerializer.DeserializeAsync<T>(stream);

        return (response.StatusCode, body)!;
    }
}
