// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions;
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Modules.Core.Shared.ClaimsHelper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace InfiniLore.Modules.Core.Shared;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableSingleton<IClaimsDtoHelper>]
public class ClaimsDtoHelper(IOptions<IdentityOptions> options) : IClaimsDtoHelper {
    public IClaimsDto GetClaimsDto(ClaimsPrincipal principal) {
        if (principal.Identity?.IsAuthenticated != true) return ClaimsDto.Empty;

        // Yes I know you could store the entire object as json and then map over all the claims.
        //      But we don't want to send over all the claims to the "unsecure" client 
        string? auth0UserId = principal.FindFirstValue(options.Value.ClaimsIdentity.UserIdClaimType);
        string? infiniloreUserId = principal.FindFirstValue(InfiniLoreClaimsStoreConstants.UserId);
        string? infiniloreUserName = principal.FindFirstValue(InfiniLoreClaimsStoreConstants.UserName);
        string? name = principal.FindFirstValue("name");
        string? email = principal.FindFirstValue("email");
        string[] roles = principal.FindAll(ClaimTypes.Role).Select(claim => claim.Value).ToArray();

        return new ClaimsDto(
            auth0UserId ?? string.Empty,
            infiniloreUserId ?? string.Empty,
            infiniloreUserName ?? string.Empty,
            name ?? string.Empty,
            email ?? string.Empty,
            roles
        );
    }

    public ClaimsPrincipal GetClaimsPrincipal<TAuthProvider>(IClaimsDto claimsDto) {
        ClaimsIdentity identity = new ClaimsIdentity(nameof(TAuthProvider))
            .AddClaim(options.Value.ClaimsIdentity.UserIdClaimType, claimsDto.Auth0UserId)
            .AddClaim("name", claimsDto.Name)
            .AddClaim(ClaimTypes.Name, claimsDto.Name)
            .AddClaim(ClaimTypes.Email, claimsDto.Email)
            .AddClaim(InfiniLoreClaimsStoreConstants.UserId, claimsDto.InfiniLoreUserId)
            .AddClaim(InfiniLoreClaimsStoreConstants.UserName, claimsDto.InfiniloreUserName)
            .AddClaim("email", claimsDto.Email);

        identity.AddClaims(claimsDto.Roles.Select(roleName => new Claim(ClaimTypes.Role, roleName)));

        return new ClaimsPrincipal(identity);
    }
}
