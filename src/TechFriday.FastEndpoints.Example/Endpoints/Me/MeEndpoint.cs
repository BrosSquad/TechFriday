using System.Security.Claims;

namespace FastEndpoints.Example.Endpoints.Me;

public class MeEndpointResponse
{
    public string Name { get; set; } = default!;
}

public class MeEndpoint : EndpointWithoutRequest<MeEndpointResponse>
{
    public override void Configure()
    {
        Get("/me");
        Policies("AdminOnly");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var name = User.FindFirstValue(ClaimTypes.NameIdentifier);

        await SendOkAsync(new MeEndpointResponse { Name = name }, cancellation: ct);
    }
}
