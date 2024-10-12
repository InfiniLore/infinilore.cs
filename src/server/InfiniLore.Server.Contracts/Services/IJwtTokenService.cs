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
    Task<JwtResult> GenerateTokensAsync(InfiniLoreUser user, string[] roles, string[] permissions, CancellationToken ct = default);
}
