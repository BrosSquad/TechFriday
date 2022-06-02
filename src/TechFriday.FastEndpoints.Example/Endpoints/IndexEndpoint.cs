using Microsoft.AspNetCore.Authorization;

namespace FastEndpoints.Example.Endpoints;

public class Response
{
    public string Message { get; set; } = default!;

    public List<string> Hosts { get; set; } = default!;
}

[HttpGet("/")]
[AllowAnonymous]
public class IndexEndpoint : EndpointWithoutRequest<Response>
{
    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendOkAsync(new Response
        {
            Message = "Welcome to TechFriday!",
            Hosts = new List<string>
            {
                "Dusan Malusev",
                "Stefan Bogdanovic"
            }
        }, ct);
    }
}