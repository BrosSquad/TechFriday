using System.Security.Claims;
using FastEndpoints.Example.Extensions;
using FastEndpoints.Example.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace FastEndpoints.Example.Endpoints.Login;

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
                    await SendAsync(new { ex.Message }, 400, cancellation: ct);
                    return;
                }
                if (exception is PasswordMissmatchException passwordMissmatchException)
                {
                    await SendAsync(new { passwordMissmatchException.Message }, 400, cancellation: ct);
                    return;
                }
            });
    }
}
