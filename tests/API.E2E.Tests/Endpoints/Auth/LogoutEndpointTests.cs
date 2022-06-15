namespace API.E2E.Tests.Endpoints.Auth;

public class LogoutEndpointTests : EndToEndTestCase
{
    protected override string Url => "/auth/logout";

    [Fact]
    public async Task Logout_No_Cookie()
    {
        // Act
        var response = await Client.PostAsync(Url, null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Logout_Successfully()
    {
        // Arrange
        await RegisterAsync(new FastEndpoints.Example.Endpoints.Users.CreateUserEndpoint.CreateUserRequest
        {
            Email = "test@test.com",
            Password = "test1234",
            FirstName = "Gang",
            LastName = "Starr"
        });

        await AuthenticateAsync(new FastEndpoints.Example.Endpoints.Auth.Login.LoginRequest
        {
            Email = "test@test.com",
            Password = "test1234"
        });

        // Act
        var response = await Client.PostAsync(Url, null);

        var authCookie = response.Headers.GetValues(HeaderNames.SetCookie).First();

        authCookie.Should().Contain("01 Jan 1970");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
