using System.Text.Json.Serialization;

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
        Response<Response>(StatusCodes.Status200OK, "Welcome message");
        Response<EmptyResponse>(StatusCodes.Status401Unauthorized, "Unauthorized");
    }
}

public class IndexEndpoint : EndpointWithoutRequest<Response>
{
    public override void Configure()
    {
        Get("/");
        //AuthSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
        //Policies("UserOnly");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendOkAsync(new Response
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
