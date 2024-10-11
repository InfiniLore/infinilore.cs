// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts.Data;
using InfiniLore.Server.Data.Models.Account;
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
    public async Task<bool> AddAsync(string userId, Guid token,DateTime expiresAt, CancellationToken ct = default) {
        return await unitOfWork.GetDbContext().Users.FindAsync([userId], cancellationToken: ct) is {} user
            && await AddAsync(user, token, expiresAt, ct);
    }
    
    public async Task<bool> AddAsync(InfiniLoreUser user, Guid token, DateTime expiresAt, CancellationToken ct = default) {
        try {
            InfiniLoreDbContext dbContext = unitOfWork.GetDbContext();
            string hashedToken = HashToken(token);
        
            if (await dbContext.JwtRefreshTokens.AnyAsync(t => t.TokenHash == hashedToken, ct)) return false;
        
            dbContext.JwtRefreshTokens.Add(new JwtRefreshToken {
                User = user,
                TokenHash = hashedToken,
                ExpiresAt = expiresAt
            });
        
            await dbContext.SaveChangesAsync(ct);
            return true;
        }
        catch (Exception ex) {
            logger.Warning(ex, "Error adding refresh token to database");
            return false;
        }
    }
    #endregion

    #region CheckAndRemoveAsync
    public async Task<bool> CheckAndRemoveAsync(string userId, Guid token, CancellationToken ct = default) => 
        await unitOfWork.GetDbContext().Users.FindAsync([userId], cancellationToken:ct) is {} user 
        && await CheckAndRemoveAsync(user, token, ct);
    
    public async Task<bool> CheckAndRemoveAsync(InfiniLoreUser user, Guid token, CancellationToken ct = default) {
        try {
            InfiniLoreDbContext dbContext = unitOfWork.GetDbContext();
            string hashedToken = HashToken(token);

            JwtRefreshToken? entry = await dbContext.JwtRefreshTokens
                .Include(jwtRefreshToken => jwtRefreshToken.User)
                .FirstOrDefaultAsync(
                    t => t.TokenHash == hashedToken
                        && t.User == user
                        && t.ExpiresAt > DateTime.UtcNow, ct
                );

            if (entry is null) return false;

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
