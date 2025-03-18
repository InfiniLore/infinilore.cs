// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Server.Services.Auth0;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace InfiniLore.ServerClient.Shared.ClaimsHelper;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IClaimsDtoHelper>(ServiceLifetime.Singleton)]
public class ClaimsDtoHelper(IOptions<IdentityOptions> options) : IClaimsDtoHelper {
    public IClaimsDto GetClaimsDto(ClaimsPrincipal principal) {
        if (principal.Identity?.IsAuthenticated != true) return ClaimsDto.Empty;

        string? auth0UserId = principal.FindFirstValue(options.Value.ClaimsIdentity.UserIdClaimType);
        string? infiniloreUserId = principal.FindFirstValue(ClaimsStoreConstants.InfiniloreUserId);
        string? infiniloreUserName = principal.FindFirstValue(ClaimsStoreConstants.InfiniloreUserName);
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
        Claim[] claims = [
            new(options.Value.ClaimsIdentity.UserIdClaimType, claimsDto.Auth0UserId),
            new("name", claimsDto.Name),
            new(ClaimTypes.Name, claimsDto.Name),
            new(ClaimTypes.Email, claimsDto.Email),
            new("email", claimsDto.Email)
        ];
        
        var identity = new ClaimsIdentity(claims, nameof(TAuthProvider));
        foreach (string roleName in claimsDto.Roles) {
            identity.AddClaim(new Claim(ClaimTypes.Role, roleName));
        }
        
        return new ClaimsPrincipal(identity);
    }
}
