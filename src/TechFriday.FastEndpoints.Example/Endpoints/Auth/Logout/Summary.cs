using FastEndpoints.Example.Extensions;

namespace FastEndpoints.Example.Endpoints.Auth.Logout;

public class LogoutEndpointSummary : Summary<LogoutEndpoint>
{
    public LogoutEndpointSummary()
    {
        Summary = "Logout endpoint";
        Description = "This endpoint logs out the user.";

        Response<Response>(StatusCodes.Status200OK, "Successful logout.");
        Response<Extensions.ErrorResponse>(StatusCodes.Status401Unauthorized, "Unauthorized.");
    }
}
