using MinimalAPI.Models;
using MinimalAPI.Requests;
using MongoDB.Driver;

namespace MinimalAPI.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetUsersAsync();
    Task<User?> GetUserAsync(string id);
    Task<User> CreateUserAsync(CreateUserRequest request);
    Task<bool> UpdateUserAsync(string id, CreateUserRequest request);
    Task<bool> DeleteUserAsync(string id);
}

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _userCollection;

    public UserRepository(IMongoCollection<User> userCollection)
    {
        _userCollection = userCollection;
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        return await (await _userCollection.FindAsync(x => true)).ToListAsync();
    }

    public async Task<User?> GetUserAsync(string id)
    {
        return await (await _userCollection.FindAsync(x => x.Id == id)).SingleOrDefaultAsync();
    }

    public async Task<User> CreateUserAsync(CreateUserRequest request)
    {
        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        await _userCollection.InsertOneAsync(user);

        return user;
    }

    public async Task<bool> UpdateUserAsync(string id, CreateUserRequest request)
    {
        var update = Builders<User>.Update
            .Set(x => x.Email, request.Email)
            .Set(x => x.Password, BCrypt.Net.BCrypt.HashPassword(request.Password))
            .Set(x => x.FirstName, request.FirstName)
            .Set(x => x.LastName, request.LastName);

        var result = await _userCollection.UpdateOneAsync(x => x.Id == id, update);

        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteUserAsync(string id)
    {
        var result = await _userCollection.DeleteOneAsync(x => x.Id == id);

        return result.DeletedCount > 0;
    }
}
