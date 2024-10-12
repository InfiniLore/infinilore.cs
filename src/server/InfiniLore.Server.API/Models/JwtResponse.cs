// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Server.API.Models;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record JwtResponse(
    [UsedImplicitly] string AccessToken,
    [UsedImplicitly] DateTime AccessTokenExpiryUtc,
    [UsedImplicitly] Guid RefreshToken,
    [UsedImplicitly] DateTime RefreshTokenExpiryUtc
);
