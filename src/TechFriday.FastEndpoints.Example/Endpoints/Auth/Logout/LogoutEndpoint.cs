using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace FastEndpoints.Example.Endpoints.Auth.Logout;

public class LogoutEndpoint : EndpointWithoutRequest
{
    public override void Configure()
    {
        Post("/auth/logout");
        AuthSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        await SendOkAsync(new { Message = "You have logged out successfully." }, ct);
    }
}
