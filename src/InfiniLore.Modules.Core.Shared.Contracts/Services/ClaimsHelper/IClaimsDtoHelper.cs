// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Security.Claims;

namespace InfiniLore.Modules.Core.Shared;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IClaimsDtoHelper {
    IClaimsDto GetClaimsDto(ClaimsPrincipal principal);
    ClaimsPrincipal GetClaimsPrincipal<TAuthProvider>(IClaimsDto claimsDto);
}
