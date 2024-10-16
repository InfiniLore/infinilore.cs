// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Server.API.Controllers.Account.JWT.Revoke;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class JwtRevokeTokensRequest {
    public required Guid[] RefreshTokens { get; [UsedImplicitly] init; } = [];
}
