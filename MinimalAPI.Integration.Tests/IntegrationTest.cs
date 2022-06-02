using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using AutoFixture;
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
        var response = await Client.GetAsync(url);

        var responseBody = await JsonSerializer.SerializeAsync(await response.Content.ReadAsStreamAsync());

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }
}
