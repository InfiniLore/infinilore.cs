// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace InfiniLore.ServerClient.Shared;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IAuthenticationStateProviderClaimsPrincipalHelper>(ServiceLifetime.Singleton)]
public class AuthenticationStateProviderClaimsPrincipalHelper(IOptions<IdentityOptions> options) : IAuthenticationStateProviderClaimsPrincipalHelper {
    public Auth0Information GetAuth0Information(ClaimsPrincipal principal) {
        if (principal.Identity?.IsAuthenticated != true) return Auth0Information.Empty;

        string? userId = principal.FindFirst(options.Value.ClaimsIdentity.UserIdClaimType)?.Value;
        string? name = principal.FindFirst("name")?.Value;
        string? email = principal.FindFirst("email")?.Value;

        return new Auth0Information(
            userId ?? string.Empty,
            name ?? string.Empty,
            email ?? string.Empty
        );
    }

    public ClaimsPrincipal GetClaimsPrincipal<TAuthProvider>(Auth0Information auth0Information) {
        Claim[] claims = [
            new(options.Value.ClaimsIdentity.UserIdClaimType, auth0Information.UserId),
            new("name", auth0Information.Name),
            new("email", auth0Information.Email)
        ];

        return new ClaimsPrincipal(new ClaimsIdentity(claims, nameof(TAuthProvider)));
    }
}
