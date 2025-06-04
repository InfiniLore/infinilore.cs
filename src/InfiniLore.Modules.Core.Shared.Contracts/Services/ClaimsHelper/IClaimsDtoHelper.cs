// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Security.Claims;

namespace InfiniLore.Modules.Core.Shared.ClaimsHelper;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IClaimsDtoHelper {
    IClaimsDto GetClaimsDto(ClaimsPrincipal principal);
    ClaimsPrincipal GetClaimsPrincipal<TAuthProvider>(IClaimsDto claimsDto);
}
