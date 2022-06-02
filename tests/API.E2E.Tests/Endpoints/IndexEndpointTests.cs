using System.Net;
using FastEndpoints.Example.Endpoints;
using FluentAssertions;

namespace API.E2E.Tests.Endpoints;

public class IndexEndpointTests : EndToEndTestCase
{
    [Fact]
    public async Task Test_Example()
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
        response.Message.Should().NotBeNull();
        response.Message.Should().Be("Welcome to TechFriday!");
        response.Hosts.Should().HaveCount(2);
        response.Hosts[0].Should().Be("Dusan Malusev");
        response.Hosts[1].Should().Be("Stefan Bogdanovic");


        status.Should().Be(HttpStatusCode.OK);
    }
}

