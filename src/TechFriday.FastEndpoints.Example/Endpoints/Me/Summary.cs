namespace FastEndpoints.Example.Endpoints.Me;

public class MeEndpointSummary : Summary<MeEndpoint>
{
    public MeEndpointSummary()
    {
        Summary = "Gets currently loggedin user's username";
        Description = "Gets currently loggedin user's username description";

        Response<MeEndpointResponse>(StatusCodes.Status200OK, "Logged in user username.");
        Response<Extensions.ErrorResponse>(StatusCodes.Status401Unauthorized, "Unauthorized.");
    }
}
