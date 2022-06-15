using FastEndpoints.Example.Extensions;
using FastEndpoints.Example.Models;

namespace FastEndpoints.Example.Endpoints.Auth.Login;

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
        Response<User>(StatusCodes.Status200OK, "User logged in sucessfully.");
        Response<List<ValidationResponse>>(StatusCodes.Status422UnprocessableEntity, "Validation errors.");
        Response<ErrorResponse>(StatusCodes.Status404NotFound, "User not found.");
    }
}
