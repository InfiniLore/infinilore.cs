// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Database.Models.Content.Account;
using InfiniLore.Server.Types;

namespace InfiniLore.Server.Contracts.Services.Auth.Authentication;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IJwtTokenGenerationService {
    ValueTask<SuccessOrFailure<JwtTokenData>> GenerateTokensAsync(InfiniLoreUser user, string[] roles, string[] permissions, int? expiresInDays, CancellationToken ct = default);
    ValueTask<SuccessOrFailure<JwtTokenData>> RefreshTokensAsync(Guid refreshToken, CancellationToken ct = default);
    ValueTask<bool> RevokeTokensAsync(InfiniLoreUser user, Guid refreshToken, CancellationToken ct = default);
    ValueTask<bool> RevokeAllTokensFromUserAsync(InfiniLoreUser user, CancellationToken ct = default);
}
