using FastEndpoints.Example.Extensions;
using FastEndpoints.Example.Models;
using MongoDB.Bson;

namespace API.E2E.Tests.Endpoints.Users;

public class GetUserEndpointTests : EndToEndTestCase
{
    protected override string Url => "/users/{id}";

    [Fact]
    public async Task Should_Get_User_Successfully()
    {
        // Arrange
        var request = new FastEndpoints.Example.Endpoints.Users.CreateUserEndpoint.CreateUserRequest
        {
            Email = "test@test.com",
            Password = "test1234",
            FirstName = "Test",
            LastName = "Test"
        };
        var userResponse = await RegisterAsync(request);

        // Act
        var response = await Client.GetAsync(Url.Replace("{id}", userResponse.Id));
        var (status, body) = await response.Extract<User>();

        // Assert
        status.Should().Be(HttpStatusCode.OK);
        body.Should().BeOfType<User>();
        body.FirstName.Should().Be(request.FirstName);
        body.LastName.Should().Be(request.LastName);
        body.Email.Should().Be(request.Email);
        body.Password.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task Should_Get_Not_Found_When_User_Does_Not_Exist()
    {
        // Arrange
        var id = ObjectId.GenerateNewId().ToString();

        // Act
        var response = await Client.GetAsync(Url.Replace("{id}", id));
        var (status, body) = await response.Extract<ErrorResponse>();

        // Assert
        status.Should().Be(HttpStatusCode.NotFound);
        body.Should().NotBeNull();
        body.Message.Should().Be("User not found.");
    }
}
