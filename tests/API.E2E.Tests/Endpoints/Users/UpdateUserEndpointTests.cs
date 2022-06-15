using FastEndpoints.Example.Endpoints.Users.CreateUserEndpoint;
using FastEndpoints.Example.Endpoints.Users.UpdateUserEndpoint;
using FastEndpoints.Example.Extensions;
using MongoDB.Bson;

namespace API.E2E.Tests.Endpoints.Users;

public class UpdateUserEndpointTests : EndToEndTestCase
{
    protected override string Url => "/users/{id}";

    [Fact]
    public async Task Should_Update_User_Successfully()
    {
        // Arrange
        var request = new CreateUserRequest
        {
            FirstName = "Test",
            LastName = "Test",
            Email = "test@test.com",
            Password = "password"
        };

        var userResponse = await RegisterAsync(request);

        var updateRequest = new UpdateUserRequest
        {
            FirstName = "Test",
            LastName = "Test",
            Email = "test@test.com",
            Password = "password"
        };

        // Act
        var response = await Client.PutAsJsonAsync(
            Url.Replace("{id}", userResponse.Id),
            updateRequest
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Validation_Errors()
    {
        // Arrange
        var request = new CreateUserRequest
        {
            FirstName = "Test",
            LastName = "Test",
            Email = "test@test.com",
            Password = "password"
        };

        var userResponse = await RegisterAsync(request);

        var updateRequest = new UpdateUserRequest
        {
            FirstName = "",
            LastName = "",
            Email = "",
            Password = ""
        };

        // Act
        var response = await Client.PutAsJsonAsync(
            Url.Replace("{id}", userResponse.Id),
            updateRequest
        );

        var (status, body) = await response.Extract<List<ValidationResponse>>();

        // Assert
        status.Should().Be(HttpStatusCode.UnprocessableEntity);
        body.Should().NotBeNull();
        body.Should().BeOfType<List<ValidationResponse>>();
        body.Should().HaveCountGreaterThan(0);
    }

    [Fact]
    public async Task User_Not_Found()
    {
        // Arrange
        var id = ObjectId.GenerateNewId().ToString();

        var updateRequest = new UpdateUserRequest
        {
            FirstName = "Test",
            LastName = "Test",
            Email = "test@test.com",
            Password = "password"
        };

        // Act
        var response = await Client.PutAsJsonAsync(
            Url.Replace("{id}", id),
            updateRequest
        );

        var (status, body) = await response.Extract<ErrorResponse>();

        // Assert
        status.Should().Be(HttpStatusCode.NotFound);
        body.Should().NotBeNull();
        body.Should().BeOfType<ErrorResponse>();
        body.Message.Should().Be("User not found.");
    }
}
