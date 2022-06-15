using FastEndpoints.Example.Services;

namespace FastEndpoints.Example.Endpoints.Users.DeleteUserEndpoint;

public class DeleteUserEndpoint : EndpointWithoutRequest
{
    private readonly IUserService _userService;

    public DeleteUserEndpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Delete("/users/{id:mongoId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<string>("id")!;

        var result = await _userService.DeleteUserAsync(id);

        if (!result)
        {
            await SendAsync(new ErrorResponse { Message = "User not found." }, cancellation: ct);
            return;
        }

        await SendNoContentAsync(ct);
    }
}
