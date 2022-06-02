using Microsoft.AspNetCore.Authorization;

namespace FastEndpoints.Example.Endpoints;

public class Response
{
    public string Uptime { get; set; } = default!;
}

[HttpGet("/")]
[AllowAnonymous]
public class IndexEndpoint : EndpointWithoutRequest<Response>
{
    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendOkAsync(new Response { Uptime = DateTime.UtcNow.ToString() }, ct);
    }
}