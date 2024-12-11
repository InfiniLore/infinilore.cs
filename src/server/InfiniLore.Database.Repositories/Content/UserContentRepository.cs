// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Database.Models;
using InfiniLore.Database.MsSqlServer;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories;
using InfiniLore.Server.Contracts.Types;
using Microsoft.EntityFrameworkCore;
using UserIdUnion=InfiniLore.Server.Contracts.Types.UserIdUnion;

namespace InfiniLore.Database.Repositories.Content;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class UserContentRepository<T>(IDbUnitOfWork<MsSqlDbContext> unitOfWork) : BaseContentRepository<T>(unitOfWork), IUserContentRepository<T> where T : UserContent {
    public async virtual ValueTask<RepoResult<T[]>> TryGetByUserAsync(UserIdUnion userUnion, CancellationToken ct = default) {
        DbSet<T> dbSet = await GetDbSetAsync(ct);
        T[] result = await dbSet
            .Where(ls => ls.OwnerId == userUnion.ToGuid())
            .ToArrayAsync(cancellationToken: ct);

        return result;
    }

    public async virtual ValueTask<RepoResult<T[]>> TryGetByUserAsync(UserIdUnion userUnion, PaginationInfo pageInfo, CancellationToken ct = default) {
        DbSet<T> dbSet = await GetDbSetAsync(ct);
        T[] result = await dbSet
            .Where(ls => ls.OwnerId == userUnion.ToGuid())
            .Skip(pageInfo.SkipAmount)
            .Take(pageInfo.PageSize)
            .ToArrayAsync(cancellationToken: ct);

        return result;
    }

    public async virtual ValueTask<RepoResult<T[]>> TryGetByUserWithUserAccessAsync(UserIdUnion ownerUnion, UserIdUnion accessorUnion, AccessKind level, CancellationToken ct = default) {
        DbSet<T> dbSet = await GetDbSetAsync(ct);

        T[] result = await dbSet
            .Where(
                model => model.OwnerId == ownerUnion.ToGuid()
                    && model.UserAccess.Any(access => access.UserId == accessorUnion.ToGuid() && access.AccessKind == level)
            )
            .ToArrayAsync(cancellationToken: ct);

        return result;
    }

    public async virtual ValueTask<RepoResult<T[]>> TryGetByUserWithUserAccessAsync(UserIdUnion ownerUnion, UserIdUnion accessorUnion, AccessKind level, PaginationInfo pageInfo, CancellationToken ct = default) {
        DbSet<T> dbSet = await GetDbSetAsync(ct);

        T[] result = await dbSet
            .Where(
                model => model.OwnerId == ownerUnion.ToGuid()
                    && model.UserAccess.Any(access => access.UserId == accessorUnion.ToGuid() && access.AccessKind == level)
            )
            .Skip(pageInfo.SkipAmount)
            .Take(pageInfo.PageSize)
            .ToArrayAsync(cancellationToken: ct);

        return result;
    }
}
