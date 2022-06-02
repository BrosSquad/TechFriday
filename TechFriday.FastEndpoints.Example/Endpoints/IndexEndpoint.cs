using Microsoft.AspNetCore.Authorization;

namespace FastEndpoints.Example.Endpoints;


[HttpGet("/")]
[AllowAnonymous]
public class IndexEndpoint : EndpointWithoutRequest
{
    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendOkAsync(new { Uptime = DateTime.UtcNow.ToString()  }, ct);
    }
}