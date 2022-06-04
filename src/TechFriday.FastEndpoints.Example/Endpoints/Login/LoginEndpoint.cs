using System.Security.Claims;
using FastEndpoints.Example.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace FastEndpoints.Example.Endpoints.Login;

[HttpPost("/auth/login")]
[AllowAnonymous]
public class LoginEndpoint : Endpoint<LoginRequest, object>
{
    private readonly ILoginService _loginService;

    public LoginEndpoint(ILoginService loginService)
    {
        _loginService = loginService;
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        var user = await _loginService.LoginAsync(req.Email, req.Password);

        if (user is null)
        {
            await SendAsync(new { Message = "User not found." }, StatusCodes.Status404NotFound, cancellation: ct);
            return;
        }

        var userClaims = new List<Claim>
        {
            new("id", user.Id),
            new(ClaimTypes.NameIdentifier, user.FirstName + " " + user.LastName),
            new(ClaimTypes.Role, user.Role ?? "Admin")
        };

        var identity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        await SendOkAsync(user, ct);
    }
}
