// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Auth0.AspNetCore.Authentication;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace InfiniLore.Modules.Core.Server.ApiEndpoints.Account;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class LoginEndpoint(IHttpContextAccessor accessor) : Endpoint<LoginRequest> {

    public override void Configure() {
        Get("/account/login");
        RoutePrefixOverride(RoutePrefixes.Empty);
        AllowAnonymous();
        DontAutoSendResponse();
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task HandleAsync(LoginRequest req, CancellationToken ct) {
        AuthenticationProperties authenticationProperties = new LoginAuthenticationPropertiesBuilder()
            .WithRedirectUri(req.RedirectUri)
            .Build();
            
        await accessor.HttpContext!.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
    }
}
