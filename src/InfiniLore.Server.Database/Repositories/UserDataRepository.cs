// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories;
using InfiniLore.Server.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace InfiniLore.Server.Database.Repositories;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class UserDataRepository<T> : BasicDataRepository<T>, IUserDataRepository<T> where T : UserData {
    protected override IQueryable<T> AutoInclude(IQueryable<T> query) 
        => query.Include(ls => ls.Owner);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async ValueTask<RepoResult<T[]>> GetByUserAsync(Guid userId, CancellationToken ct = default) {
        // Access
        DbSet<T> dbSet = GetDbSet<T>();

        // Query
        IQueryable<T> query = dbSet.Where(ls => ls.OwnerId == userId);

        // Retrieve
        T[] result = await query.ToArrayAsync(cancellationToken: ct);
        return RepoResult<T[]>.FromSuccess(result);
    }

    public async ValueTask<RepoResult<T[]>> GetByUserWithAutoIncludeAsync(Guid userId, CancellationToken ct = default) {
        // Access
        DbSet<T> dbSet = GetDbSet<T>();

        // Query
        IQueryable<T> query = dbSet
            .Where(ls => ls.OwnerId == userId)
            .With(AutoInclude);

        // Retrieve
        T[] result = await query.ToArrayAsync(cancellationToken: ct);
        return RepoResult<T[]>.FromSuccess(result);
    }

    public async ValueTask<RepoResult<T[]>> GetByUserReverseAsync(Guid userId, CancellationToken ct = default) {
        // Access
        DbSet<T> dbSet = GetDbSet<T>();

        // Query
        IQueryable<T> query = dbSet
            .OrderByDescending(ls => ls.Id)
            .Reverse()
            .Where(ls => ls.OwnerId == userId);

        // Retrieve
        T[] result = await query.ToArrayAsync(cancellationToken: ct);
        return RepoResult<T[]>.FromSuccess(result);
    }

    public async ValueTask<RepoResult<T[]>> GetByUserReverseWithAutoIncludeAsync(Guid userId, CancellationToken ct = default) {
        // Access
        DbSet<T> dbSet = GetDbSet<T>();

        // Query
        IQueryable<T> query = dbSet
            .OrderByDescending(ls => ls.Id)
            .Reverse()
            .Where(ls => ls.OwnerId == userId)
            .With(AutoInclude);

        // Retrieve
        T[] result = await query.ToArrayAsync(cancellationToken: ct);
        return RepoResult<T[]>.FromSuccess(result);
    }

    public async ValueTask<PaginatedRepoResult<T>> GetByUserAsync(Guid userId, PaginationInfo pageInfo, CancellationToken ct = default) {
        // Access
        DbSet<T> dbSet = GetDbSet<T>();

        // Query
        int totalCount = await dbSet.CountAsync(ct);
        if (totalCount == 0) return PaginatedResult<T>.Empty;

        IQueryable<T> query = dbSet
            .OrderByDescending(ls => ls.Id)
            .Where(ls => ls.OwnerId == userId)
            .Skip(pageInfo.SkipAmount)
            .Take(pageInfo.PageSize);

        // Retrieve
        T[] data = await query.ToArrayAsync(cancellationToken: ct);
        return new PaginatedResult<T>(
            data,
            totalCount,
            pageInfo.PageNumber,
            (int)Math.Ceiling(totalCount / (double)pageInfo.PageSize)
        );
    }
    public async ValueTask<PaginatedRepoResult<T>> GetByUserWithAutoIncludeAsync(Guid userId, PaginationInfo pageInfo, CancellationToken ct = default) {
        // Access
        DbSet<T> dbSet = GetDbSet<T>();

        // Query
        int totalCount = await dbSet.CountAsync(ct);
        if (totalCount == 0) return PaginatedResult<T>.Empty;

        IQueryable<T> query = dbSet
            .OrderByDescending(ls => ls.Id)
            .Where(ls => ls.OwnerId == userId)
            .Skip(pageInfo.SkipAmount)
            .Take(pageInfo.PageSize)
            .With(AutoInclude);

        // Retrieve
        T[] data = await query.ToArrayAsync(cancellationToken: ct);
        return new PaginatedResult<T>(
            data,
            totalCount,
            pageInfo.PageNumber,
            (int)Math.Ceiling(totalCount / (double)pageInfo.PageSize)
        );
    }
    public async ValueTask<PaginatedRepoResult<T>> GetByUserReverseAsync(Guid userId, PaginationInfo pageInfo, CancellationToken ct = default) {
        // Access
        DbSet<T> dbSet = GetDbSet<T>();

        // Query
        int totalCount = await dbSet.CountAsync(ct);
        if (totalCount == 0) return PaginatedResult<T>.Empty;

        IQueryable<T> query = dbSet
            .OrderByDescending(ls => ls.Id)
            .Where(ls => ls.OwnerId == userId)
            .Reverse()
            .Skip(pageInfo.SkipAmount)
            .Take(pageInfo.PageSize);

        // Retrieve
        T[] data = await query.ToArrayAsync(cancellationToken: ct);
        return new PaginatedResult<T>(
            data,
            totalCount,
            pageInfo.PageNumber,
            (int)Math.Ceiling(totalCount / (double)pageInfo.PageSize)
        );
    }

    public async ValueTask<PaginatedRepoResult<T>> GetByUserReverseWithAutoIncludeAsync(Guid userId, PaginationInfo pageInfo, CancellationToken ct = default) {
        // Access
        DbSet<T> dbSet = GetDbSet<T>();

        // Query
        int totalCount = await dbSet.CountAsync(ct);
        if (totalCount == 0) return PaginatedResult<T>.Empty;

        IQueryable<T> query = dbSet
            .OrderByDescending(ls => ls.Id)
            .Reverse()
            .Skip(pageInfo.SkipAmount)
            .Take(pageInfo.PageSize)
            .With(AutoInclude);

        // Retrieve
        T[] data = await query.ToArrayAsync(cancellationToken: ct);
        return new PaginatedResult<T>(
            data,
            totalCount,
            pageInfo.PageNumber,
            (int)Math.Ceiling(totalCount / (double)pageInfo.PageSize)
        );
    }
}
