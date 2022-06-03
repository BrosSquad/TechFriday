using FastEndpoints.Example.Models;
using FastEndpoints.Example.Services;
using Microsoft.AspNetCore.Authorization;

namespace FastEndpoints.Example.Endpoints.Users.GetUsersEndpoint;

[HttpGet("/users")]
[AllowAnonymous]
public class GetUsersEndpoint : EndpointWithoutRequest<List<User>>
{
    private readonly IUserService _userService;

    public GetUsersEndpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var users = await _userService.GetUsersAsync();

        await SendAsync(users, cancellation: ct);
    }
}
