// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Data.Models.Account;

namespace InfiniLore.Server.Contracts.Repositories;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IJwtRefreshTokenRepository {
    public Task<bool> AddAsync(string userId, Guid token, DateTime expiresAt, CancellationToken ct = default);
    public Task<bool> AddAsync(InfiniLoreUser user, Guid token, DateTime expiresAt, CancellationToken ct = default);
    public Task<bool> CheckAndRemoveAsync(string userId, Guid token, CancellationToken ct = default);
    public Task<bool> CheckAndRemoveAsync(InfiniLoreUser user, Guid token, CancellationToken ct = default);
}
