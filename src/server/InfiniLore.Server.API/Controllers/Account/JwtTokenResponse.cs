// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using InfiniLore.Database.Models.Content.Account;
using InfiniLore.Server.Types;

namespace InfiniLore.Server.API.Controllers.Account;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable NotAccessedPositionalProperty.Global
public record JwtTokenResponse(
    Guid OwnerId,
    string AccessToken,
    DateTime AccessTokenExpiryUtc,
    Guid RefreshToken,
    DateTime RefreshTokenExpiryUtc,
    string[] Permissions,
    string[] Roles
) {
    public static JwtTokenResponse FromModel(InfiniLoreUser user, JwtTokenData model) => new(
        user.Id,
        model.AccessToken,
        model.AccessTokenExpiryUtc,
        model.RefreshToken,
        model.RefreshTokenExpiryUtc,
        model.Permissions,
        model.Roles
    );
}
