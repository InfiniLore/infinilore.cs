// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection;
using AterraEngine.Unions;
using InfiniLore.Database.Models.Content.Account;
using InfiniLore.Database.MsSqlServer;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories;
using InfiniLore.Server.Contracts.Database.Repositories.Content.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography;
using System.Text;
using RepoResult=InfiniLore.Server.Contracts.Database.Repositories.RepoResult;
using UserIdUnion=InfiniLore.Server.Contracts.Types.UserIdUnion;

namespace InfiniLore.Database.Repositories.Content.Account;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IJwtRefreshTokenRepository>(ServiceLifetime.Scoped)]
public class JwtRefreshTokenRepository(IUnitOfWork unitOfWork) : IJwtRefreshTokenRepository {

    #region Queries
    public async ValueTask<RepoResult<JwtRefreshTokenModel>> TryGetByIdAsync(Guid refreshtoken, CancellationToken ct = default) {
        var dbContext = await unitOfWork.GetDbContextAsync<MsSqlDbContext>(ct);
        string hashedToken = HashToken(refreshtoken);

        JwtRefreshTokenModel? tokenData = await dbContext.JwtRefreshTokens
            .Include(t => t.Owner)
            .FirstOrDefaultAsync(predicate: t => t.TokenHash == hashedToken, ct);

        if (tokenData == null) {
            return "Token not found.";
        }

        return tokenData;
    }
    #endregion

    private static string HashToken(Guid token) {
        byte[] tokenBytes = Encoding.UTF8.GetBytes(token.ToString());
        byte[] hashBytes = SHA256.HashData(tokenBytes);
        return Convert.ToBase64String(hashBytes);
    }

    #region Commands
    public async ValueTask<RepoResult> TryAddAsync(JwtRefreshTokenModel model, CancellationToken ct = default) {
        var dbContext = await unitOfWork.GetDbContextAsync<MsSqlDbContext>(ct);
        if (await dbContext.JwtRefreshTokens.AnyAsync(predicate: m => m.Id == model.Id, ct)) return "Model already exists";

        await dbContext.JwtRefreshTokens.AddAsync(model, ct);
        return new Success();
    }

    public async ValueTask<RepoResult<JwtRefreshTokenModel>> TryAddWithResultAsync(JwtRefreshTokenModel model, CancellationToken ct = default) {
        var dbContext = await unitOfWork.GetDbContextAsync<MsSqlDbContext>(ct);
        if (await dbContext.JwtRefreshTokens.AnyAsync(predicate: m => m.Id == model.Id, ct)) return "Model already exists";

        EntityEntry<JwtRefreshTokenModel> result = await dbContext.JwtRefreshTokens.AddAsync(model, ct);
        return result.Entity;
    }

    public async ValueTask<RepoResult> TryAddRangeAsync(IEnumerable<JwtRefreshTokenModel> models, CancellationToken ct = default) {
        var dbContext = await unitOfWork.GetDbContextAsync<MsSqlDbContext>(ct);
        if (await dbContext.JwtRefreshTokens.AnyAsync(predicate: m => models.Any(m2 => m2.Id == m.Id), ct)) return "One or more Models already exist";

        await dbContext.JwtRefreshTokens.AddRangeAsync(models, ct);
        return new Success();
    }

    public async ValueTask<RepoResult> TryPermanentRemoveAllForUserAsync(UserIdUnion userUnion, CancellationToken ct = default) {
        var dbContext = await unitOfWork.GetDbContextAsync<MsSqlDbContext>(ct);
        var userId = userUnion.ToGuid();

        int recordsAffected = await dbContext.JwtRefreshTokens
            .Where(m => m.OwnerId == userId)
            .ExecuteDeleteAsync(cancellationToken: ct);

        if (recordsAffected <= 0) return "No models were deleted";

        return new Success();
    }

    public async ValueTask<RepoResult> TryRemoveAsync(JwtRefreshTokenModel model, CancellationToken ct = default) {
        var dbContext = await unitOfWork.GetDbContextAsync<MsSqlDbContext>(ct);
        JwtRefreshTokenModel? existing = await dbContext.JwtRefreshTokens.FindAsync([model.Id], ct);
        if (existing == null) return "Model does not exist";

        dbContext.JwtRefreshTokens.Remove(existing);
        return new Success();
    }

    public async ValueTask<RepoResult> TryRemoveRangeAsync(IEnumerable<JwtRefreshTokenModel> models, CancellationToken ct = default) {
        var dbContext = await unitOfWork.GetDbContextAsync<MsSqlDbContext>(ct);
        HashSet<Guid> ids = models.Select(model => model.Id).ToHashSet();

        int recordsAffected = await dbContext.JwtRefreshTokens
            .Where(model => ids.Contains(model.Id))
            .ExecuteDeleteAsync(ct);

        if (recordsAffected <= 0 && ids.Count != 0) return "No models were deleted";

        return new Success();
    }
    #endregion
}
