using FastEndpoints.Example.Models;

namespace FastEndpoints.Example.Endpoints.Users.GetUserEndpoint;

public class GetUserEndpointSummary : Summary<GetUserEndpoint>
{
    public GetUserEndpointSummary()
    {
        Summary = "Get user endpoint";
        Description = "Get User By Id Endpoint";

        Response<User>(StatusCodes.Status200OK, "User found.");
        Response<ErrorResponse>(StatusCodes.Status404NotFound, "User not found.");
    }
}
