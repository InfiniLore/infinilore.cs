// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Data.Models.Account;

namespace InfiniLore.Server.Contracts.Data.Repositories.Commands;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IJwtRefreshTokenCommands {
    public Task<bool> AddAsync(string userId, Guid token, DateTime expiresAt, string[] roles, string[] permissions, int? expiresInDays, CancellationToken ct = default);
    public Task<bool> AddAsync(InfiniLoreUser user, Guid token, DateTime expiresAt, string[] roles, string[] permissions, int? expiresInDays, CancellationToken ct = default);

    public Task<bool> DeleteAsync(Guid token, CancellationToken ct = default);
    public Task<bool> DeleteAsync(JwtRefreshToken token, CancellationToken ct = default);

    public Task<bool> DeleteAllAsync(string userId, CancellationToken ct = default);
}
