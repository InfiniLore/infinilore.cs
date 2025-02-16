// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Extensions.DependencyInjection;
using CodeOfChaos.Types.UnitOfWork;
using Old.InfiniLore.Database.Models.Content.Account;
using Old.InfiniLore.Contracts.Database.Repositories.Content.Account;
using Old.InfiniLore.Server.Types;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;

namespace Old.InfiniLore.Database.Repositories.Content.Account;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IJwtRefreshTokenRepository>(ServiceLifetime.Scoped)]
public class JwtRefreshTokenRepository: UnitOfWorkRepository<ContentDbContext>, IJwtRefreshTokenRepository {

    public async ValueTask<RepoResult<JwtRefreshTokenModel>> TryGetByHashedTokenAsync(string hashedToken, CancellationToken ct = default) {
        ContentDbContext dbContext = GetDbContext();

        JwtRefreshTokenModel? tokenData = await dbContext.JwtRefreshTokens
            .Include(t => t.Owner)
            .FirstOrDefaultAsync(predicate: t => t.TokenHash == hashedToken, ct);

        if (tokenData == null) {
            return "Token not found.";
        }

        return tokenData;
    }

    public async ValueTask<RepoResult> TryAddAsync(JwtRefreshTokenModel model, CancellationToken ct = default) {
        ContentDbContext dbContext = GetDbContext();
        if (await dbContext.JwtRefreshTokens.AnyAsync(predicate: m => m.Id == model.Id, ct)) return "Model already exists";

        await dbContext.JwtRefreshTokens.AddAsync(model, ct);
        return new Success();
    }

    public async ValueTask<RepoResult<JwtRefreshTokenModel>> TryAddWithResultAsync(JwtRefreshTokenModel model, CancellationToken ct = default) {
        ContentDbContext dbContext = GetDbContext();
        if (await dbContext.JwtRefreshTokens.AnyAsync(predicate: m => m.Id == model.Id, ct)) return "Model already exists";

        EntityEntry<JwtRefreshTokenModel> result = await dbContext.JwtRefreshTokens.AddAsync(model, ct);
        return result.Entity;
    }

    public async ValueTask<RepoResult> TryAddRangeAsync(IEnumerable<JwtRefreshTokenModel> models, CancellationToken ct = default) {
        ContentDbContext dbContext = GetDbContext();
        if (await dbContext.JwtRefreshTokens.AnyAsync(predicate: m => models.Any(m2 => m2.Id == m.Id), ct)) return "One or more Models already exist";

        await dbContext.JwtRefreshTokens.AddRangeAsync(models, ct);
        return new Success();
    }

    public async ValueTask<RepoResult> TryPermanentRemoveAllForUserAsync(Guid userId, CancellationToken ct = default) {
        ContentDbContext dbContext = GetDbContext();

        int recordsAffected = await dbContext.JwtRefreshTokens
            .Where(m => m.OwnerId == userId)
            .ExecuteDeleteAsync(cancellationToken: ct);

        if (recordsAffected <= 0) return "No models were deleted";

        return new Success();
    }

    public async ValueTask<RepoResult> TryRemoveAsync(JwtRefreshTokenModel model, CancellationToken ct = default) {
        ContentDbContext dbContext = GetDbContext();
        JwtRefreshTokenModel? existing = await dbContext.JwtRefreshTokens.FindAsync([model.Id], ct);
        if (existing == null) return "Model does not exist";

        dbContext.JwtRefreshTokens.Remove(existing);
        return new Success();
    }

    public async ValueTask<RepoResult> TryRemoveRangeAsync(IEnumerable<JwtRefreshTokenModel> models, CancellationToken ct = default) {
        ContentDbContext dbContext = GetDbContext();
        HashSet<Guid> ids = models.Select(model => model.Id).ToHashSet();

        int recordsAffected = await dbContext.JwtRefreshTokens
            .Where(model => ids.Contains(model.Id))
            .ExecuteDeleteAsync(ct);

        if (recordsAffected <= 0 && ids.Count != 0) return "No models were deleted";

        return new Success();
    }
}
