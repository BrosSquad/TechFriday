namespace FastEndpoints.Example.Services;

public interface IHasherService
{
	bool Verify(string password, string hash);
	string Generate(string password);
}

public class HasherService : IHasherService
{
    public string Generate(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool Verify(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}

