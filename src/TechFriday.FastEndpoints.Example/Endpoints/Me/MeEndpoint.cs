using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace FastEndpoints.Example.Endpoints.Me;

public class MeEndpointResponse
{
    public string Name { get; set; } = default!;
}

public class MeEndpointSummary : Summary<MeEndpoint>
{
    public MeEndpointSummary()
    {
        Summary = "Gets currently loggedin user's username";
        Description = "Gets currently loggedin user's username description";
        Response<MeEndpointResponse>(StatusCodes.Status200OK, "Logged in user username");
        Response<object>(StatusCodes.Status401Unauthorized, "Unauthorized");
    }
}

[HttpGet("/me")]
[Authorize(Policy = "AdminOnly")]
public class MeEndpoint : EndpointWithoutRequest<MeEndpointResponse>
{
    public override async Task HandleAsync(CancellationToken ct)
    {
        var name = User.FindFirstValue(ClaimTypes.NameIdentifier);

        await SendOkAsync(new MeEndpointResponse { Name = name }, cancellation: ct);
    }
}
