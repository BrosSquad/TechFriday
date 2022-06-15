using FastEndpoints.Example.Extensions;
using MongoDB.Bson;

namespace API.E2E.Tests.Endpoints.Users;

public class DeleteUserEndpointTests : EndToEndTestCase
{
    protected override string Url => "/users/{id}";

    [Fact]
    public async Task Should_Delete_User_Successfully()
    {
        // Arrange
        var userResponse = await RegisterAsync(new FastEndpoints.Example.Endpoints.Users.CreateUserEndpoint.CreateUserRequest
        {
            FirstName = "test",
            LastName = "test",
            Email = "test@test.com",
            Password = "password"
        });

        // Act
        var response = await Client.DeleteAsync(Url.Replace("{id}", userResponse.Id));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Should_Not_Delete_User_When_User_Does_Not_Exist()
    {
        // Arrange
        var id = ObjectId.GenerateNewId().ToString();

        // Act
        var response = await Client.DeleteAsync(Url.Replace("{id}", id));

        var (status, body) = await response.Extract<ErrorResponse>();

        // Assert
        status.Should().Be(HttpStatusCode.NotFound);
        body.Should().NotBeNull();
        body.Should().BeOfType<ErrorResponse>();
        body.Message.Should().Be("User not found.");
    }
}
