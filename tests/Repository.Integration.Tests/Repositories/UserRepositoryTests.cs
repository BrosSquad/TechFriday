using FastEndpoints.Example.Endpoints.Users.CreateUserEndpoint;
using FastEndpoints.Example.Models;
using FastEndpoints.Example.Repositories;

namespace Repository.Integration.Tests.Repositories;

public class UserRepositoryTests : IntegrationTestCase
{
    private readonly UserRepository _userRepository;

    public UserRepositoryTests()
    {
        var collection = Database.GetCollection<User>(nameof(User));
        
        _userRepository = new UserRepository(collection);
    }

    [Fact]
    public async Task GetUsersAsync_Should_Return_Empty_Array()
    {
        var users = await _userRepository.GetUsersAsync();

        users.Should().BeEmpty();
    }

    [Fact]
    public async Task GetUsersAsync_Should_Results_Successfully()
    {
        // Arrange
        var user = new CreateUserRequest
        {
            FirstName = "Test",
            LastName = "Test",
            Email = "test@test.com",
            Password = "test"
        };

        await _userRepository.CreateUserAsync(user);

        // Act
        var users = await _userRepository.GetUsersAsync();

        // Assert
        users.Should().NotBeEmpty();
        users.Should().HaveCount(1);
        users.Should().BeOfType<List<User>>();
    }

    [Fact]
    public async Task GetUserAsync_Should_Results_Successfully()
    {
        // Arrange
        var request = new CreateUserRequest
        {
            FirstName = "Test",
            LastName = "Test",
            Email = "test@test.com",
            Password = "test"
        };

        var result = await _userRepository.CreateUserAsync(request);

        // Act
        var user = await _userRepository.GetUserAsync(result.Id);

        // Assert
        user.Should().NotBeNull();
        user.Should().BeOfType<User>();
    }

    [Fact]
    public async Task GetUserAsync_Should_Return_Null_When_User_Is_Not_Found()
    {
        // Arrange
        var Id = ObjectId.GenerateNewId().ToString();

        // Act
        var user = await _userRepository.GetUserAsync(Id);

        // Assert
        user.Should().BeNull();
    }

    [Fact]
    public async Task CreateUserAsync_Should_Insert_User_When_Data_Is_Valid()
    {
        // Arrange
        var user = new CreateUserRequest
        {
            FirstName = "Test",
            LastName = "Test",
            Email = "test@test.com",
            Password = BCrypt.Net.BCrypt.HashPassword("test")
        };

        // Act
        var result = await _userRepository.CreateUserAsync(user);

        // Assert
        result.Should().BeOfType<User>();
        result.Should().NotBeNull();
        result.Id.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateUserAsync_Should_Update_User_Successfully_When_Data_Is_Valid()
    {
        // Arrange
        var user = new CreateUserRequest
        {
            FirstName = "Test",
            LastName = "Test",
            Email = "test@test.com",
            Password = "test"
        };

        var result = await _userRepository.CreateUserAsync(user);

        // Act
        var update = await _userRepository.UpdateUserAsync(result.Id, new CreateUserRequest
        {
            FirstName = "Test Update",
            LastName = "Test Update",
            Email = "test@update.com",
            Password = "test123"
        });


        // Assert
        update.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteUserAsync_Should_Delete_User_Successfully()
    {
        // Arrange
        var user = new CreateUserRequest
        {
            FirstName = "Test",
            LastName = "Test",
            Email = "test@test.com",
            Password = "test"
        };

        var result = await _userRepository.CreateUserAsync(user);

        // Act
        var deleteResult = await _userRepository.DeleteUserAsync(result.Id);


        // Assert
        deleteResult.Should().BeTrue();
    }

    [Fact]
    public async Task GetUserByEmailAsync_Should_Return_User_Successfully_When_Email_Exists()
    {
        // Arrange
        var request = new CreateUserRequest
        {
            FirstName = "Test",
            LastName = "Test",
            Email = "test@test.com",
            Password = "test"
        };

        var result = await _userRepository.CreateUserAsync(request);

        // Act
        var user = await _userRepository.GetUserByEmailAsync(result.Email);

        // Assert
        user.Should().BeOfType<User>();
        user.Should().NotBeNull();
    }

    [Fact]
    public async Task GetUserByEmailAsync_Should_Return_Null_When_Email_Does_Not_Exists()
    {
        // Arrange
        var request = new CreateUserRequest
        {
            FirstName = "Test",
            LastName = "Test",
            Email = "test@test.com",
            Password = "test"
        };

        await _userRepository.CreateUserAsync(request);

        // Act
        var user = await _userRepository.GetUserByEmailAsync("random-email@gmail.com");

        // Assert
        user.Should().BeNull();
    }
}
