namespace FastEndpoints.Example.Endpoints.Users.CreateUserEndpoint;

public class CreateUserRequest
{
    public string FirstName {get; set;} = default!;
    public string LastName {get; set;} = default!;
    public string Email {get; set;} = default!;
    public string Password {get; set;} = default!;
}
