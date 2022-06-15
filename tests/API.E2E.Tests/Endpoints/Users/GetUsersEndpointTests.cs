using FastEndpoints.Example.Endpoints.Users.CreateUserEndpoint;
using FastEndpoints.Example.Models;

namespace API.E2E.Tests.Endpoints.Users;

public class GetUsersEndpointTests : EndToEndTestCase
{
    protected override string Url => "/users";

    [Fact]
    public async Task Should_Get_Empty_Users()
    {
        var response = await Client.GetAsync(Url);

        var (status, body) = await response.Extract<List<User>>();

        status.Should().Be(HttpStatusCode.OK);
        body.Should().BeEmpty();
    }

    [Fact]
    public async Task Should_Get_Users()
    {
        // Arrange
        var request = new CreateUserRequest
        {
            FirstName = "Test" + Guid.NewGuid().ToString(),
            LastName = "Test" + Guid.NewGuid().ToString(),
            Email = "test@test.com",
            Password = "password"
        };

        var createUserResponse = await Client.PostAsJsonAsync("/users", request);
        createUserResponse.EnsureSuccessStatusCode();

        // Act
        var response = await Client.GetAsync(Url);

        var (status, body) = await response.Extract<List<User>>();

        // Assert
        status.Should().Be(HttpStatusCode.OK);
        body.Should().BeOfType<List<User>>();
        body.Should().HaveCount(1);
    }
}
