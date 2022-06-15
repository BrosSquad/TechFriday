using FastEndpoints.Example.Models;

namespace FastEndpoints.Example.Endpoints.Users.GetUsersEndpoint;

public class GetUsersEndpointSummary : Summary<GetUsersEndpoint>
{
    public GetUsersEndpointSummary()
    {
        Summary = "Get users endpoint.";
        Description = "This endpoint gets users.";

        Response<List<User>>(StatusCodes.Status200OK, "List of users.");
    }
}