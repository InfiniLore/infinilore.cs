// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Data.Models.Account;

namespace InfiniLore.Server.Contracts.Repositories;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IJwtRefreshTokenRepository {
    public Task<bool> AddAsync(string userId, Guid token, DateTime expiresAt, string[] roles, string[] permissions, int? expiresInDays, CancellationToken ct = default);
    public Task<bool> AddAsync(InfiniLoreUser user, Guid token, DateTime expiresAt, string[] roles, string[] permissions, int? expiresInDays, CancellationToken ct = default);
    
    public Task<JwtRefreshToken?> GetAsync(Guid token, CancellationToken ct = default);
    public Task<bool> RemoveAsync(Guid token, CancellationToken ct = default);
    public Task<bool> RemoveAsync(JwtRefreshToken token, CancellationToken ct = default);
}
