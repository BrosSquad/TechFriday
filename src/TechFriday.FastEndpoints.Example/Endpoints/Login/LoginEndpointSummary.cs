using FastEndpoints.Example.Models;

namespace FastEndpoints.Example.Endpoints.Login;

public class LoginEndpointSummary : Summary<LoginEndpoint>
{
    public LoginEndpointSummary()
    {
        Summary = "Login endpoint (Exception)";
        Description = "This endpoint logs in the user, and sets cookie.";
        ExampleRequest = new LoginRequest
        {
            Email = "test@test.com",
            Password = "password"
        };
        Response<User>(200, "User logged in sucessfully.");
        Response<List<Extensions.ErrorResponse>>(StatusCodes.Status422UnprocessableEntity, "Validation errors.");
        Response<object>(StatusCodes.Status404NotFound, "User not found.");
    }
}
