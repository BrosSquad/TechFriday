using FastEndpoints.Example.Models;
using FastEndpoints.Example.Services;

namespace FastEndpoints.Example.Endpoints.Users.GetUsersEndpoint;

public class GetUsersEndpoint : EndpointWithoutRequest<List<User>>
{
    private readonly IUserService _userService;

    public GetUsersEndpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Get("/users");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var users = await _userService.GetUsersAsync();

        await SendAsync(users, cancellation: ct);
    }
}
