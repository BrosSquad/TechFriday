namespace FastEndpoints.Example.Endpoints.Users.CreateUserEndpoint;

public class Summary : Summary<CreateUserEndpoint>
{
    public Summary()
    {
        ExampleRequest = new
        {
            FirstName = "Test",
            LastName = "Test"
        };
    }
}
