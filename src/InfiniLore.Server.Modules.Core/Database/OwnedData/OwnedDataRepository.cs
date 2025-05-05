// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Server.Modules.Core.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace InfiniLore.Server.Modules.Core.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class OwnedDataRepository<TOwner, TModel, TInterface> : BasicDataRepository<TModel, TInterface>, IOwnedDataRepository<TOwner, TInterface> 
    where TModel : OwnedData<TOwner>, TInterface 
    where TInterface: IOwnedData<TOwner>
    where TOwner : class, IBasicData 
{
    protected override IQueryable<TModel> AutoInclude(IQueryable<TModel> query)
        => query.Include(ls => ls.Owner);
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async ValueTask<Result<TInterface[]>> GetByOwnerAsync(Guid userId, QueryConfig config = default, CancellationToken ct = default) {
        // Access
        DbSet<TModel> dbSet = GetDbSet<TModel>();

        // Query
        IQueryable<TInterface> query = dbSet
            .ConditionalWith(config.AutoInclude, AutoInclude)
            .ConditionalReverse(config.Reverse)
            .Where(ls => ls.OwnerId == userId);

        // Retrieve
        TInterface[] result = await query.ToArrayAsync(cancellationToken: ct);
        return Result<TInterface[]>.FromSuccess(result);
    }

    public async ValueTask<PaginatedResult<TInterface>> GetByOwnerAsync(Guid userId, PaginationInfo pageInfo, QueryConfig config = default, CancellationToken ct = default) {
        // Access
        DbSet<TModel> dbSet = GetDbSet<TModel>();

        // Query
        IQueryable<TModel> baseQuery = dbSet.Where(ls => ls.OwnerId == userId);

        int totalCount = await baseQuery.CountAsync(ct);
        if (totalCount == 0) return PaginatedData<TInterface>.Empty;

        IQueryable<TInterface> query = baseQuery
            .ConditionalWith(config.AutoInclude, AutoInclude)
            .ConditionalReverse(config.Reverse)
            .OrderByDescending(ls => ls.Id)
            .Skip(pageInfo.SkipAmount)
            .Take(pageInfo.PageSize);

        // Retrieve
        TInterface[] data = await query.ToArrayAsync(cancellationToken: ct);
        return new PaginatedData<TInterface>(
            data,
            totalCount,
            pageInfo.PageNumber,
            (int)Math.Ceiling(totalCount / (double)pageInfo.PageSize)
        );
    }
}
