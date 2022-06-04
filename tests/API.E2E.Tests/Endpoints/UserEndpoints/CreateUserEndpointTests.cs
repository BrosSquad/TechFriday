using FastEndpoints.Example.Endpoints.Users.CreateUserEndpoint;

namespace API.E2E.Tests.Endpoints.UserEndpoints;

public class CreateUserEndpointTests : EndToEndTestCase
{
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
        var response = await Client.PostAsJsonAsync("/users", request);

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
        var response = await Client.PostAsJsonAsync("/users", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }
}
