using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace FastEndpoints.Example.Endpoints;

public class IndexResponse
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

        Response<IndexResponse>(StatusCodes.Status200OK, "Welcome message.");
        Response<Extensions.ErrorResponse>(StatusCodes.Status401Unauthorized, "Unauthorized.");
    }
}

public class IndexEndpoint : EndpointWithoutRequest<IndexResponse>
{
    public override void Configure()
    {
        Get("/");
        AuthSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
        Policies("UserOnly");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendOkAsync(new IndexResponse
        {
            Message = "Welcome to TechFriday!",
            Hosts = new List<string>
            {
                "Dusan Malušev",
                "Stefan Bogdanović"
            }
        }, ct);
    }
}
