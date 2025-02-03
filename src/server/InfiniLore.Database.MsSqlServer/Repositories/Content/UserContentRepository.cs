// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Database.Models;
using InfiniLore.Server.Contracts.Database.Repositories;
using InfiniLore.Server.Types;

namespace InfiniLore.Database.MsSqlServer.Repositories.Content;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class UserContentRepository<T> : BasicContentRepository<T>, IUserContentRepository<T> where T : UserContent {

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public virtual async ValueTask<RepoResult<T[]>> TryGetByUserAsync(Guid userId, CancellationToken ct = default) {
        DbSet<T> dbSet = await GetDbSetAsync(ct);
        
        T[] result = await dbSet
            .Where(ls => ls.OwnerId == userId)
            .ToArrayAsync(cancellationToken: ct);

        return result;
    }

    public virtual async ValueTask<RepoResult<T[]>> TryGetByUserAsync(Guid userId, PaginationInfo pageInfo, CancellationToken ct = default) {
        DbSet<T> dbSet = await GetDbSetAsync(ct);
        
        T[] result = await dbSet
            .Where(ls => ls.OwnerId == userId)
            .Skip(pageInfo.SkipAmount)
            .Take(pageInfo.PageSize)
            .ToArrayAsync(cancellationToken: ct);

        return result;
    }

    public virtual async ValueTask<RepoResult<T[]>> TryGetByUserWithUserAccessAsync(Guid ownerId, Guid accessorId, AccessKind level, CancellationToken ct = default) {
        DbSet<T> dbSet = await GetDbSetAsync(ct);
        
        T[] result = await dbSet
            .Where(
                model => model.OwnerId == ownerId
                    && model.UserAccess.Any(access => access.UserId == accessorId && access.AccessKind == level)
            )
            .ToArrayAsync(cancellationToken: ct);

        return result;
    }

    public virtual async ValueTask<RepoResult<T[]>> TryGetByUserWithUserAccessAsync(Guid ownerId, Guid accessorId, AccessKind level, PaginationInfo pageInfo, CancellationToken ct = default) {
        DbSet<T> dbSet = await GetDbSetAsync(ct);

        T[] result = await dbSet
            .Where(
                model => model.OwnerId == ownerId
                    && model.UserAccess.Any(access => access.UserId == accessorId && access.AccessKind == level)
            )
            .Skip(pageInfo.SkipAmount)
            .Take(pageInfo.PageSize)
            .ToArrayAsync(cancellationToken: ct);

        return result;
    }
}
