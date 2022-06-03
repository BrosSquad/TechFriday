using FastEndpoints.Example.Services;
using Microsoft.AspNetCore.Authorization;

namespace FastEndpoints.Example.Endpoints.Users.CreateUserEndpoint;

[HttpPost("/users")]
[AllowAnonymous]
public class CreateUserEndpoint : Endpoint<CreateUserRequest, CreateUserResponse>
{
    private readonly IUserService _userService;

    public CreateUserEndpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override async Task HandleAsync(CreateUserRequest req, CancellationToken ct)
    {
        var user = await _userService.CreateUserAsync(req);

        await SendAsync(new CreateUserResponse { Id = user.Id }, StatusCodes.Status201Created, cancellation: ct);
    }
}
