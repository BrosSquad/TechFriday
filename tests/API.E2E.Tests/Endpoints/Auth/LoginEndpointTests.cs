using API.E2E.Tests.Extensions;
using FastEndpoints.Example.Endpoints.Login;

namespace API.E2E.Tests.Endpoints.Auth;

public class LoginEndpointTests : EndToEndTestCase
{
        [Fact]
		public async Task Error_When_User_Not_Found()
        {
            // Arrange
            const string url = "/auth/login";
            var user = Fixture.Build<LoginRequest>()
                .With(x => x.Email, "test@test.com")
                .With(x => x.Password, "password")
                .Create();

            // Act
            var response = await Client.PostAsJsonAsync(url, user);
            var authCookie = response.ExtractCookie("Auth");
                
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            authCookie.Should().NotBeNull();
            authCookie.Should().BeOfType<string>();
        }
}

