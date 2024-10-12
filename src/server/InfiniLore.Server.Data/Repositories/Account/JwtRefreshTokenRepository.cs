// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts.Data;
using InfiniLore.Server.Contracts.Data.Repositories;
using InfiniLore.Server.Data.Models.Account;
using Microsoft.Data.Sqlite;
using Serilog;
using System.Security.Cryptography;
using System.Text;

namespace InfiniLore.Server.Data.Repositories.Account;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[RegisterService<IJwtRefreshTokenRepository>(LifeTime.Scoped)]
public class JwtRefreshTokenRepository(IDbUnitOfWork<InfiniLoreDbContext> unitOfWork, ILogger logger) : IJwtRefreshTokenRepository {

    private static string HashToken(Guid token) {
        byte[] tokenBytes = Encoding.UTF8.GetBytes(token.ToString());
        byte[] hashBytes = SHA256.HashData(tokenBytes);
        return Convert.ToBase64String(hashBytes);
    }

    #region AddAsync
    public async Task<bool> AddAsync(string userId, Guid token, DateTime expiresAt, string[] roles, string[] permissions, int? expiresInDays, CancellationToken ct = default) {
        InfiniLoreDbContext dbContext = unitOfWork.GetDbContext();
        InfiniLoreUser? user = await dbContext.Users.FindAsync([userId], ct);

        if (user != null) return await AddAsync(user, token, expiresAt, roles, permissions, expiresInDays, ct);

        logger.Warning("User with ID {UserId} not found", userId);
        return false;

    }

    public async Task<bool> AddAsync(InfiniLoreUser user, Guid token, DateTime expiresAt, string[] roles, string[] permissions, int? expiresInDays, CancellationToken ct = default) {
        try {
            InfiniLoreDbContext dbContext = unitOfWork.GetDbContext();
            string hashedToken = HashToken(token);

            bool tokenExists = await dbContext.JwtRefreshTokens.AnyAsync(predicate: t => t.TokenHash == hashedToken, ct);
            if (tokenExists) {
                logger.Warning("Refresh token already exists for user {UserId}", user.Id);
                return false;
            }

            user.JwtRefreshTokens.Add(new JwtRefreshToken {
                User = user,
                TokenHash = hashedToken,
                ExpiresAt = expiresAt,
                Roles = roles,
                Permissions = permissions,
                ExpiresInDays = expiresInDays
            });

            dbContext.Users.Update(user);

            await dbContext.SaveChangesAsync(ct);
            return true;
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqliteException { SqliteErrorCode: 19 } sqliteEx) {
            logger.Warning(ex, "Unique constraint violation: {Message}", sqliteEx.Message);
            return false;
        }
        catch (Exception ex) {
            logger.Warning(ex, "Error adding refresh token to database");
            return false;
        }
    }
    #endregion
    
    #region GetAsync
    public async Task<JwtRefreshToken?> GetAsync(Guid token, CancellationToken ct = default) {
        InfiniLoreDbContext dbContext = unitOfWork.GetDbContext();
        string hashedToken = HashToken(token);

        JwtRefreshToken? tokenData = await dbContext.JwtRefreshTokens
            .Include(t => t.User)
            .FirstOrDefaultAsync(predicate: t => t.TokenHash == hashedToken, ct);

        return tokenData;
    }
    #endregion
    
    #region RemoveAsync
    public async Task<bool> RemoveAsync(Guid token, CancellationToken ct = default) {
        if (await GetAsync(token, ct) is not {} tokenData) return false;

        return await RemoveAsync(tokenData, ct);
    }

    public async Task<bool> RemoveAsync(JwtRefreshToken token, CancellationToken ct = default) {
        InfiniLoreDbContext dbContext = unitOfWork.GetDbContext();

        dbContext.JwtRefreshTokens.Remove(token);
        await dbContext.SaveChangesAsync(ct);

        return true;
    }
    #endregion
    
    #region RemoveAllAsync
    public async Task<bool> RemoveAllAsync(string userId, CancellationToken ct = default) {
        InfiniLoreDbContext dbContext = unitOfWork.GetDbContext();

        int recordAffected = await dbContext.JwtRefreshTokens
            .Where(t => t.User.Id == userId)
            .ExecuteDeleteAsync(cancellationToken: ct);

        return recordAffected > 0;
    }
    #endregion
}
