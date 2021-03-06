using FastEndpoints.Example.Endpoints.Auth.Login;
using FastEndpoints.Example.Extensions;
using FastEndpoints.Example.Models;

namespace API.E2E.Tests.Endpoints.Auth;

public class LoginEndpointTests : EndToEndTestCase
{
    protected override string Url => "/auth/login";

    [Fact]
    public async Task Error_When_User_Not_Found()
    {
        // Arrange
        var user = new LoginRequest
        {
            Email = "not-found@test.com",
            Password = "password"
        };

        // Act
        var response = await Client.PostAsJsonAsync(Url, user);

        var (status, body) = await response.Extract<ErrorResponse>();

        // Assert
        status.Should().Be(HttpStatusCode.NotFound);
        body.Should().NotBeNull();
        body.Message.Should().Be("User not found.");
    }

    [Fact]
    public async Task Login_Success()
    {
        // Arrange
        await RegisterAsync(new FastEndpoints.Example.Endpoints.Users.CreateUserEndpoint.CreateUserRequest
        {
            Email = "test@test.com",
            Password = "test1234",
            FirstName = "Gang",
            LastName = "Starr"
        });


        var user = new LoginRequest
        {
            Email = "test@test.com",
            Password = "test1234"
        };

        // Act
        var response = await Client.PostAsJsonAsync(Url, user);

        var authCookie = response.Headers.GetValues(HeaderNames.SetCookie).First();

        var (status, body) = await response.Extract<User>();

        // Assert
        authCookie.Should().NotBeEmpty();
        authCookie.Should().NotBeNull();
        authCookie.Should().BeOfType<string>();

        status.Should().Be(HttpStatusCode.OK);
        body.Should().NotBeNull();
        body.Should().BeOfType<User>();
    }

    [Fact]
    public async Task Validation_Errors()
    {
        // Arrange
        var user = new LoginRequest
        {
            Email = "",
            Password = ""
        };

        // Act
        var response = await Client.PostAsJsonAsync(Url, user);

        var (status, body) = await response.Extract<List<ValidationResponse>>();

        // Assert
        status.Should().Be(HttpStatusCode.UnprocessableEntity);
        body.Should().NotBeNull();
        body.Should().BeOfType<List<ValidationResponse>>();
        body.Should().HaveCount(4);
    }
}
