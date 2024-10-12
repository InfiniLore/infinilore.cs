// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts.Data;
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
    public async Task<bool> AddAsync(string userId, Guid token, DateTime expiresAt, CancellationToken ct = default) {
        InfiniLoreDbContext dbContext = unitOfWork.GetDbContext();
        InfiniLoreUser? user = await dbContext.Users.FindAsync([userId], ct);

        if (user != null) return await AddAsync(user, token, expiresAt, ct);

        logger.Warning("User with ID {UserId} not found", userId);
        return false;

    }

    public async Task<bool> AddAsync(InfiniLoreUser user, Guid token, DateTime expiresAt, CancellationToken ct = default) {
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
                ExpiresAt = expiresAt
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

    #region CheckAndRemoveAsync
    public async Task<bool> CheckAndRemoveAsync(string userId, Guid token, CancellationToken ct = default) {
        InfiniLoreDbContext dbContext = unitOfWork.GetDbContext();
        InfiniLoreUser? user = await dbContext.Users.FindAsync(new object[] { userId }, ct);

        if (user != null) return await CheckAndRemoveAsync(user, token, ct);

        logger.Warning("User with ID {UserId} not found", userId);
        return false;

    }

    public async Task<bool> CheckAndRemoveAsync(InfiniLoreUser user, Guid token, CancellationToken ct = default) {
        try {
            InfiniLoreDbContext dbContext = unitOfWork.GetDbContext();
            string hashedToken = HashToken(token);

            JwtRefreshToken? entry = await dbContext.JwtRefreshTokens
                .Include(t => t.User)
                .FirstOrDefaultAsync(
                    predicate: t => t.TokenHash == hashedToken
                        && t.User == user
                        && t.ExpiresAt > DateTime.UtcNow, ct);

            if (entry == null) {
                logger.Warning("No valid refresh token found for user {UserId}", user.Id);
                return false;
            }

            dbContext.JwtRefreshTokens.Remove(entry);
            await dbContext.SaveChangesAsync(ct);

            return true;
        }
        catch (Exception ex) {
            logger.Warning(ex, "Error removing refresh token from database");
            return false;
        }
    }
    #endregion
}
