// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Server.Contracts.Services.ClaimsPrincipalHelper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace InfiniLore.Server.Services.ClaimsPrincipalHelper;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IClaimsPrincipalHelper>(ServiceLifetime.Singleton)]
public class ClaimsPrincipalHelper(IOptions<IdentityOptions> options) : IClaimsPrincipalHelper {
    public IAuth0Information GetAuth0Information(ClaimsPrincipal principal) {
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
}
