// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Server.Contracts;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories;
using InfiniLore.Server.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace InfiniLore.Server.Database.Repositories;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class ProjectDataRepository<T> : BasicDataRepository<T>, IProjectDataRepository<T> where T : ProjectData {

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async ValueTask<Result<T[]>> GetByLoreScopeAsync(Guid loreScopeId, QueryConfig config = default, CancellationToken ct = default) {
        // Access
        DbSet<T> dbSet = GetDbSet<T>();

        // Query
        IQueryable<T> query = dbSet
            .ConditionalWith(config.AutoInclude, AutoInclude)
            .ConditionalReverse(config.Reverse)
            .Where(ls => ls.LoreScopeId == loreScopeId);

        // Retrieve
        T[] result = await query.ToArrayAsync(cancellationToken: ct);
        return Result<T[]>.FromSuccess(result);
    }

    public async ValueTask<PaginatedResult<T>> GetByLoreScopeAsync(Guid loreScopeId, PaginationInfo pageInfo, QueryConfig config = default, CancellationToken ct = default) {
        // Access
        DbSet<T> dbSet = GetDbSet<T>();

        // Query
        IQueryable<T> baseQuery = dbSet.Where(ls => ls.LoreScopeId == loreScopeId);

        int totalCount = await baseQuery.CountAsync(ct);
        if (totalCount == 0) return PaginatedData<T>.Empty;

        IQueryable<T> query = baseQuery
            .ConditionalWith(config.AutoInclude, AutoInclude)
            .ConditionalReverse(config.Reverse)
            .OrderByDescending(ls => ls.Id)
            .Skip(pageInfo.SkipAmount)
            .Take(pageInfo.PageSize);

        // Retrieve
        T[] data = await query.ToArrayAsync(cancellationToken: ct);
        return new PaginatedData<T>(
            data,
            totalCount,
            pageInfo.PageNumber,
            (int)Math.Ceiling(totalCount / (double)pageInfo.PageSize)
        );
    }

    protected override IQueryable<T> AutoInclude(IQueryable<T> query)
        => query.Include(ls => ls.LoreScope);
}
