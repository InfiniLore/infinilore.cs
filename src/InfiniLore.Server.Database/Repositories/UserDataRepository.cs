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
    public async ValueTask<RepoResult<T[]>> GetByUserAsync(Guid userId, QueryConfig config = default, CancellationToken ct = default) {
        // Access
        DbSet<T> dbSet = GetDbSet<T>();

        // Query
        IQueryable<T> query = dbSet
            .ConditionalWith(config.AutoInclude, AutoInclude)
            .ConditionalReverse(config.Reverse)
            .Where(ls => ls.OwnerId == userId);

        // Retrieve
        T[] result = await query.ToArrayAsync(cancellationToken: ct);
        return RepoResult<T[]>.FromSuccess(result);
    }

    public async ValueTask<PaginatedRepoResult<T>> GetByUserAsync(Guid userId, PaginationInfo pageInfo,  QueryConfig config = default, CancellationToken ct = default) {
        // Access
        DbSet<T> dbSet = GetDbSet<T>();

        // Query
        IQueryable<T> baseQuery = dbSet.Where(ls => ls.OwnerId == userId);
        
        int totalCount = await baseQuery.CountAsync(ct);
        if (totalCount == 0) return PaginatedResult<T>.Empty;

        IQueryable<T> query = baseQuery
            .ConditionalWith(config.AutoInclude, AutoInclude)
            .ConditionalReverse(config.Reverse)
            .OrderByDescending(ls => ls.Id)
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
}
