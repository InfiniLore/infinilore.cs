// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Security.Claims;

namespace InfiniLore.ServerClient.Shared;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IAuthenticationStateProviderClaimsPrincipalHelper {
    Auth0Information GetAuth0Information(ClaimsPrincipal principal);
    ClaimsPrincipal GetClaimsPrincipal<TAuthProvider>(Auth0Information auth0Information);
}
