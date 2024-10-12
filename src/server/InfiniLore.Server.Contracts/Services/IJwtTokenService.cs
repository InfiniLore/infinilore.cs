// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Data.Models.Account;
using InfiniLoreLib.Results;

namespace InfiniLore.Server.Contracts.Services;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IJwtTokenService {
    Task<JwtResult> GenerateTokensAsync(InfiniLoreUser user, string[] roles, string[] permissions, int? expiresInDays, CancellationToken ct = default);
    Task<JwtResult> RefreshTokensAsync(Guid refreshToken, CancellationToken ct = default);
    Task<BoolResult> RevokeTokensAsync(InfiniLoreUser user, Guid refreshToken, CancellationToken ct = default);
    Task<BoolResult> RevokeAllTokensFromUserAsync(InfiniLoreUser user, CancellationToken ct = default);
}
