// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts.Data;
using InfiniLore.Server.Contracts.Data.Repositories.Queries;
using InfiniLore.Server.Data.Models.Account;
using System.Security.Cryptography;
using System.Text;

namespace InfiniLore.Server.Data.Repositories.Query.Account;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[RegisterService<IJwtRefreshTokenQueries>(LifeTime.Scoped)]
public class JwtRefreshTokenQueries(IDbUnitOfWork<InfiniLoreDbContext> unitOfWork) : IJwtRefreshTokenQueries {
    private static string HashToken(Guid token) {
        byte[] tokenBytes = Encoding.UTF8.GetBytes(token.ToString());
        byte[] hashBytes = SHA256.HashData(tokenBytes);
        return Convert.ToBase64String(hashBytes);
    }
    
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
}
