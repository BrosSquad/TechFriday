using FastEndpoints.Example.Endpoints.Users.CreateUserEndpoint;
using FastEndpoints.Example.Models;
using MongoDB.Driver;

namespace FastEndpoints.Example.Repositories;

public interface IUserRepository
{
    Task<List<User>> GetUsersAsync();
    Task<User?> GetUserAsync(string id);
    Task<User> CreateUserAsync(CreateUserRequest request);
    Task<bool> UpdateUserAsync(string id, CreateUserRequest request);
    Task<bool> DeleteUserAsync(string id);
    Task<User?> GetUserByEmailAsync(string email);
}

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _userCollection;

    public UserRepository(IMongoCollection<User> userCollection)
    {
        _userCollection = userCollection;
    }

    public async Task<List<User>> GetUsersAsync()
    {
        return await (await _userCollection.FindAsync(x => true)).ToListAsync();
    }

    public async Task<User?> GetUserAsync(string id)
    {
        return await (await _userCollection.FindAsync(x => x.Id == id)).SingleOrDefaultAsync();
    }

    public async Task<User> CreateUserAsync(CreateUserRequest request)
    {
        // We are using this to randomize role (showcase only)
        var random = new Random();

        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = random.NextSingle() > 0.5 ? "Admin" : "User"
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

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await (await _userCollection.FindAsync(x => x.Email == email)).FirstOrDefaultAsync();
    }
}
