using FastEndpoints.Example.Endpoints.Users.CreateUserEndpoint;
using FastEndpoints.Example.Extensions;

namespace API.E2E.Tests.Endpoints.Users;

public class CreateUserEndpointTests : EndToEndTestCase
{
    protected override string Url => "/users";

    [Fact]
    public async Task Create_Successfully()
    {
        // Arrange
        var request = new CreateUserRequest
        {
            FirstName = "Test" + Guid.NewGuid().ToString(),
            LastName = "Test" + Guid.NewGuid().ToString(),
            Email = "test@test.com",
            Password = "password"
        };

        // Act
        var response = await Client.PostAsJsonAsync(Url, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Validation_Will_Fail()
    {
        // Arrange
        var request = new CreateUserRequest
        {
            FirstName = "Test" + Guid.NewGuid().ToString(),
            LastName = "Test" + Guid.NewGuid().ToString(),
            Email = "",
            Password = ""
        };

        // Act
        var response = await Client.PostAsJsonAsync(Url, request);

        var (status, body) = await response.Extract<List<ValidationResponse>>();

        // Assert
        status.Should().Be(HttpStatusCode.UnprocessableEntity);
        body.Should().NotBeNull();
    }
}
