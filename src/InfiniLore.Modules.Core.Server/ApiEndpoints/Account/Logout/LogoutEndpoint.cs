// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Auth0.AspNetCore.Authentication;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace InfiniLore.Modules.Core.Server.ApiEndpoints.Account;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class LogoutEndpoint(IHttpContextAccessor accessor) : Endpoint<LogoutRequest> {
    
    public override void Configure() {
        Get("/account/logout");
        RoutePrefixOverride(RoutePrefixes.Empty);
        AllowAnonymous();
        DontAutoSendResponse();
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task HandleAsync(LogoutRequest req, CancellationToken ct) {
        AuthenticationProperties authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
            .WithRedirectUri(req.RedirectUri)
            .Build();

        await accessor.HttpContext!.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        await accessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}
