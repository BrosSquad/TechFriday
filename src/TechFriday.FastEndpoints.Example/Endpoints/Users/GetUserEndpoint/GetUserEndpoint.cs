using FastEndpoints.Example.Services;
using Microsoft.AspNetCore.Authorization;

namespace FastEndpoints.Example.Endpoints.Users.GetUserEndpoint;

[HttpGet("/users/{id:mongoId}")]
[AllowAnonymous]
public class GetUserEndpoint : EndpointWithoutRequest<object>
{
    private readonly IUserService _userService;

    public GetUserEndpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<string>("id");

        var user = await _userService.GetUserAsync(id!);

        if (user is null)
        {
            await SendAsync(new { Message = "User not found" }, StatusCodes.Status404NotFound, ct);
            return;
        }

        await SendOkAsync(user, cancellation: ct);
    }
}
