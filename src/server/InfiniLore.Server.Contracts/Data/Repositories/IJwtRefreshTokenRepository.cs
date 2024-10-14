// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Data.Models.Account;

namespace InfiniLore.Server.Contracts.Data.Repositories;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IJwtRefreshTokenRepository {
    public Task<bool> AddAsync(string userId, Guid token, DateTime expiresAt, string[] roles, string[] permissions, int? expiresInDays, CancellationToken ct = default);
    public Task<bool> AddAsync(InfiniLoreUser user, Guid token, DateTime expiresAt, string[] roles, string[] permissions, int? expiresInDays, CancellationToken ct = default);

    public Task<JwtRefreshToken?> GetAsync(Guid token, CancellationToken ct = default);
    public Task<bool> DeleteAsync(Guid token, CancellationToken ct = default);
    public Task<bool> DeleteAsync(JwtRefreshToken token, CancellationToken ct = default);

    public Task<bool> DeleteAllAsync(string userId, CancellationToken ct = default);
}
