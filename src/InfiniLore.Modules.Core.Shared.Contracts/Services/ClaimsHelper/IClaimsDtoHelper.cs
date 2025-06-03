// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Security.Claims;

namespace InfiniLore.Shared.Services.ClaimsHelper;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IClaimsDtoHelper {
    IClaimsDto GetClaimsDto(ClaimsPrincipal principal);
    ClaimsPrincipal GetClaimsPrincipal<TAuthProvider>(IClaimsDto claimsDto);
}
