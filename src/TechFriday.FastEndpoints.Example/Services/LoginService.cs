
using FastEndpoints.Example.Models;
using FastEndpoints.Example.Repositories;

namespace FastEndpoints.Example.Services;

public interface ILoginService
{
    Task<User?> LoginAsync(string email, string password);
}

public class LoginService : ILoginService
{
    private readonly IUserRepository _userRepository;
    private readonly IHasherService _hasherService;

    public LoginService(IUserRepository userRepository, IHasherService hasherService)
    {
        _userRepository = userRepository;
        _hasherService = hasherService;
    }


    public async Task<User?> LoginAsync(string email, string password)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);

        if (user is null)
        {
            return null;
        }

        if (!_hasherService.Verify(password, user.Password))
        {
            throw new Exception("Passwords do not match.");
        }

        return user;
    }
}

