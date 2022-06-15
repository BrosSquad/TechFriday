using FastEndpoints.Example.Services;

namespace FastEndpoints.Example.Endpoints.Users.UpdateUserEndpoint;

public class UpdateUserEndpoint : Endpoint<UpdateUserRequest, object>
{
    private readonly IUserService _userService;

    public UpdateUserEndpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Put("/users/{id:mongoId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateUserRequest req, CancellationToken ct)
    {
        var id = Route<string>("id")!;

        var result = await _userService.UpdateUserAsync(id, new CreateUserEndpoint.CreateUserRequest
        {
            Email = req.Email,
            Password = req.Password,
            FirstName = req.FirstName,
            LastName = req.LastName
        });

        if (!result)
        {
            await SendAsync(new ErrorResponse
            {
                Message = "User not found."
            }, StatusCodes.Status404NotFound, cancellation: ct);

            return;
        }

        await SendNoContentAsync(ct);
    }
}
