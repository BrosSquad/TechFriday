using System.Security.Claims;
using FastEndpoints.Example.Extensions;
using FastEndpoints.Example.Models;
using FastEndpoints.Example.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace FastEndpoints.Example.Endpoints.Auth.Login;

public class LoginEndpointWithoutExceptionsSummary : Summary<LoginEndpointWithoutExceptions>
{
    public LoginEndpointWithoutExceptionsSummary()
    {
        Summary = "Login endpoint (Language Extensions)";
        Description = "This endpoint logs in the user, and sets cookie.";

        ExampleRequest = new LoginRequest
        {
            Email = "test@test.com",
            Password = "password"
        };

        Response<User>(StatusCodes.Status200OK, "User logged in sucessfully.");
        Response<List<ValidationResponse>>(StatusCodes.Status422UnprocessableEntity, "Validation errors.");
        Response<Extensions.ErrorResponse>(StatusCodes.Status404NotFound, "User not found.");
    }
}

public class LoginEndpointWithoutExceptions : Endpoint<LoginRequest, object>
{
    private readonly ILoginServiceExt _loginService;

    public LoginEndpointWithoutExceptions(ILoginServiceExt loginService)
    {
        _loginService = loginService;
    }

    public override void Configure()
    {
        Post("/login/ext");
        AllowAnonymous();
        DontThrowIfValidationFails();
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        if (ValidationFailed)
        {
            await SendAsync(ValidationFailures.ToResponse(), StatusCodes.Status422UnprocessableEntity, ct);
            return;
        }

        var result = await _loginService.LoginAsync(req.Email, req.Password);

        _ = result.Match(async user =>
            {
                var userClaims = new List<Claim>
                {
                    new("id", user.Id),
                    new(ClaimTypes.NameIdentifier, user.FirstName + " " + user.LastName)
                };

                var identity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            }, async exception =>
            {
                if (exception is UserNotFoundException ex)
                {
                    await SendAsync(new ErrorResponse { Message = ex.Message }, 400, cancellation: ct);
                    return;
                }
                if (exception is PasswordMissmatchException passwordMissmatchException)
                {
                    await SendAsync(new ErrorResponse { Message = passwordMissmatchException.Message }, 400, cancellation: ct);
                    return;
                }
            });
    }
}
