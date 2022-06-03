using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;

namespace FastEndpoints.Example.Endpoints;

public class Response
{
    [JsonPropertyName("message")]
    public string Message { get; set; } = default!;

    [JsonPropertyName("hosts")]
    public List<string> Hosts { get; set; } = default!;
}

public class IndexEndpointSummary : Summary<IndexEndpoint>
{
    public IndexEndpointSummary()
    {
        Summary = "Welcome endpoint";
        Description = "Welcome endpoint example";
        Response<Response>(200, "Welcome message");
    }
}

[HttpGet("/")]
[Authorize(Policy = "UserOnly")]
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