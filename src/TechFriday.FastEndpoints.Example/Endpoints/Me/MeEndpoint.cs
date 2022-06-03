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
        Summary = "";
        Description = "";
        Response<MeEndpointResponse>(200, "Logged in user username");
    }
}

[HttpGet("/me")]
[Authorize]
public class MeEndpoint : EndpointWithoutRequest<MeEndpointResponse>
{
    public override async Task HandleAsync(CancellationToken ct)
    {
        var name = User.FindFirstValue(ClaimTypes.NameIdentifier);

        await SendOkAsync(new MeEndpointResponse { Name = name }, cancellation: ct);
    }
}
