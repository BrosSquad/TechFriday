using FastEndpoints.Example.Models;
using FastEndpoints.Example.Repositories;
using FastEndpoints.Example.Services;
using NSubstitute.ReturnsExtensions;

namespace Service.Unit.Tests;

public class LoginServiceTests
{
    private readonly LoginService _sut;
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IHasherService _hasherService = Substitute.For<IHasherService>();

    private readonly Fixture fixture = new();

    public LoginServiceTests()
    {
        _sut = new LoginService(_userRepository, _hasherService);
    }

    [Fact]
    public async Task LoginSuccess()
    {
        // Arrange
        var user = fixture.Create<User>();
        const string Email = "test@test.com";
        const string Password = "Password";
        _userRepository.GetUserByEmailAsync(Email).Returns(user);
        _hasherService.Verify(Password, user.Password).Returns(true);

        // Act
        var result = await _sut.LoginAsync(Email, Password);

        // Assert
        result.Should().NotBeNull()
            .And
            .BeOfType<User>();
    }

    [Fact]
    public async Task LoginError()
    {
        // Arrange
        _userRepository.GetUserByEmailAsync(Arg.Any<string>()).ReturnsNull();
        _hasherService.DidNotReceive().Verify(Arg.Any<string>(), Arg.Any<string>());

        // Act
        var result = await _sut.LoginAsync("test@test.com", "password");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task WillThrowException()
    {
        // Arrange
        var user = fixture.Create<User>();
        const string Email = "test@test.com";
        const string Password = "Password";
        _userRepository.GetUserByEmailAsync(Email).Returns(user);
        _hasherService.Verify(Password, user.Password).Returns(false);

        // Act
        var result = async () => await _sut.LoginAsync(Email, Password);

        // Assert
        await result.Should().ThrowAsync<Exception>().WithMessage("Passwords do not match.");
    }
}
