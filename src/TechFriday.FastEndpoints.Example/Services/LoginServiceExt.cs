using FastEndpoints.Example.Models;
using FastEndpoints.Example.Repositories;
using LanguageExt.Common;

namespace FastEndpoints.Example.Services;

public interface ILoginServiceExt
{
	Task<Result<User>> LoginAsync(string email, string password);
}

public class UserNotFoundException : Exception
{
    public UserNotFoundException() : base("User not found.")
    {

    }
}

public class PasswordMissmatchException : Exception
{
    public PasswordMissmatchException() : base("Passwords do not match.")
    {

    }
}

public class LoginServiceExt : ILoginServiceExt
{
    private readonly IUserRepository _userRepository;
    private readonly IHasherService _hasherService;

    public LoginServiceExt(IUserRepository userRepository, IHasherService hasherService)
    {
        _userRepository = userRepository;
        _hasherService = hasherService;
    }


    public async Task<Result<User>> LoginAsync(string email, string password)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);

        if (user is null)
        {
            return new Result<User>(new UserNotFoundException());
        }

        if (!_hasherService.Verify(password, user.Password))
        {
            return new Result<User>(new PasswordMissmatchException());
        }

        return new Result<User>(user);
    }
}

