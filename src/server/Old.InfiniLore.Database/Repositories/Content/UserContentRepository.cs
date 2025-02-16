// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Old.InfiniLore.Database.Models;
using Old.InfiniLore.Contracts.Database.Repositories;
using Old.InfiniLore.Server.Types;

namespace Old.InfiniLore.Database.Repositories.Content;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class UserContentRepository<T>: BasicContentRepository<T>, IUserContentRepository<T> where T : UserContent {

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public virtual async ValueTask<RepoResult<T[]>> TryGetByUserAsync(Guid userId, CancellationToken ct = default) {
        DbSet<T> dbSet = GetDbSet<T>();

        IQueryable<T> query = dbSet.Where(ls => ls.OwnerId == userId);
        IncludeOnGet(query);
        
        T[] result = await query
            .ToArrayAsync(cancellationToken: ct);

        return result;
    }

    public virtual async ValueTask<RepoResult<T[]>> TryGetByUserAsync(Guid userId, PaginationInfo pageInfo, CancellationToken ct = default) {
        DbSet<T> dbSet = GetDbSet<T>();

        IQueryable<T> query = dbSet
            .Where(ls => ls.OwnerId == userId)
            .Skip(pageInfo.SkipAmount)
            .Take(pageInfo.PageSize);
        IncludeOnGet(query);

        T[] result = await query
            .ToArrayAsync(cancellationToken: ct);

        return result;
    }

    public virtual async ValueTask<RepoResult<T[]>> TryGetByUserWithUserAccessAsync(Guid ownerId, Guid accessorId, AccessKind level, CancellationToken ct = default) {
        DbSet<T> dbSet = GetDbSet<T>();

        IQueryable<T> query = dbSet.Where(
                model => model.OwnerId == ownerId
                    && model.UserAccess.Any(access => access.UserId == accessorId && access.AccessKind == level)
            );
        IncludeOnGet(query);

        T[] result = await query
            .ToArrayAsync(cancellationToken: ct);

        return result;
    }

    public virtual async ValueTask<RepoResult<T[]>> TryGetByUserWithUserAccessAsync(Guid ownerId, Guid accessorId, AccessKind level, PaginationInfo pageInfo, CancellationToken ct = default) {
        DbSet<T> dbSet = GetDbSet<T>();

        IQueryable<T> query = dbSet
            .Where(
                model => model.OwnerId == ownerId
                    && model.UserAccess.Any(access => access.UserId == accessorId && access.AccessKind == level)
            )
            .Skip(pageInfo.SkipAmount)
            .Take(pageInfo.PageSize);
        IncludeOnGet(query);

        T[] result = await query
            .ToArrayAsync(cancellationToken: ct);

        return result;
    }
}
