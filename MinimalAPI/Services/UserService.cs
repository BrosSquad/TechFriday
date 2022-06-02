using MinimalAPI.Models;
using MinimalAPI.Repositories;
using MinimalAPI.Requests;

namespace MinimalAPI.Services;

public interface IUserService
{
    Task<IEnumerable<User>> GetUsersAsync();
    Task<User?> GetUserAsync(string id);
    Task<User> CreateUserAsync(CreateUserRequest request);
    Task<bool> UpdateUserAsync(string id, CreateUserRequest request);
    Task<bool> DeleteUserAsync(string id);
}

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        return await _userRepository.GetUsersAsync();
    }

    public async Task<User?> GetUserAsync(string id)
    {
        var user = await _userRepository.GetUserAsync(id);

        return user;
    }

    public Task<User> CreateUserAsync(CreateUserRequest request)
    {
        return _userRepository.CreateUserAsync(request);
    }

    public async Task<bool> UpdateUserAsync(string id, CreateUserRequest request)
    {
        return await _userRepository.UpdateUserAsync(id, request);
    }

    public async Task<bool> DeleteUserAsync(string id)
    {
        return await _userRepository.DeleteUserAsync(id);
    }
}
