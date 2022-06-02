using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace FastEndpoints.Example.Endpoints.Me;

[HttpGet("/me")]
[Authorize]
public class MeEndpoint : EndpointWithoutRequest
{
    public override async Task HandleAsync(CancellationToken ct)
    {
        var name = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        await SendOkAsync(new { Name = name }, cancellation: ct);
    }
}

