using FastEndpoints.Example.Endpoints;
using FastEndpoints.Example.Extensions;

namespace API.E2E.Tests.Endpoints;

public class IndexEndpointTests : EndToEndTestCase
{
    protected override string Url => "/";

    [Fact]
    public async Task Should_Get_Information_Successfully()
    {
        // Arrange
        var request = new FastEndpoints.Example.Endpoints.Users.CreateUserEndpoint.CreateUserRequest
        {
            Email = "test@test.com",
            Password = "password",
            LastName = "User",
            FirstName = "User"
        };

        await RegisterAsync(request);

        await AuthenticateAsync(new FastEndpoints.Example.Endpoints.Auth.Login.LoginRequest
        {
            Email = request.Email,
            Password = request.Password
        });

        // Act
        var response = await Client.GetAsync(Url);
        var (status, body) = await response.Extract<IndexResponse>();

        // Assert
        status.Should().Be(HttpStatusCode.OK);
        body.Should().NotBeNull();
        body.Message.Should().Be("Welcome to TechFriday!");
        body.Hosts.Should().HaveCount(2);
        body.Hosts[0].Should().Be("Dusan Malušev");
        body.Hosts[1].Should().Be("Stefan Bogdanović");
    }

    [Fact]
    public async Task Should_Get_Unauthorized_When_Invalid_Role()
    {
        // Arrange
        var request = new FastEndpoints.Example.Endpoints.Users.CreateUserEndpoint.CreateUserRequest
        {
            Email = "test@test.com",
            Password = "password",
            LastName = "Admin",
            FirstName = "Admin"
        };

        await RegisterAsync(request);

        await AuthenticateAsync(new FastEndpoints.Example.Endpoints.Auth.Login.LoginRequest
        {
            Email = request.Email,
            Password = request.Password
        });

        // Act
        var response = await Client.GetAsync(Url);
        var (status, body) = await response.Extract<ErrorResponse>();

        // Assert
        status.Should().Be(HttpStatusCode.Unauthorized);
        body.Should().NotBeNull();
        body.Message.Should().Be("Unauthorized.");
    }
}
