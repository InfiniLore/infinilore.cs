// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Shared;
using Microsoft.EntityFrameworkCore;

namespace InfiniLore.Modules.Core.Server.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class OwnedModelRepository<TOwner, TModel> : BasicModelRepository<TModel>, IOwnedModelRepository<TOwner, TModel> 
    where TModel : OwnedModel<TOwner>
    where TOwner : BasicModel 
{
    protected override IQueryable<TModel> OptionalInclude(IQueryable<TModel> query)
        => base.OptionalInclude(query)
            .Include(ls => ls.Owner);
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async ValueTask<Result<TModel[]>> GetByOwnerAsync(Guid userId, QueryConfig config = default, CancellationToken ct = default) {
        // Access
        DbSet<TModel> dbSet = GetDbSet<TModel>();

        // Query
        IQueryable<TModel> query = GetConfiguredQueryable(dbSet, config)
            .Where(ls => ls.OwnerId == userId);

        // Retrieve
        TModel[] result = await query.ToArrayAsync(cancellationToken: ct);
        return Result<TModel[]>.FromSuccess(result);
    }

    public async ValueTask<PaginatedResult<TModel>> GetByOwnerAsync(Guid userId, PaginationInfo pageInfo, QueryConfig config = default, CancellationToken ct = default) {
        // Access
        DbSet<TModel> dbSet = GetDbSet<TModel>();

        // Query
        IQueryable<TModel> baseQuery = dbSet.Where(ls => ls.OwnerId == userId);

        int totalCount = await baseQuery.CountAsync(ct);
        if (totalCount == 0) return PaginatedData<TModel>.Empty;

        IQueryable<TModel> query = GetConfiguredQueryable(baseQuery, config)
            .OrderByDescending(ls => ls.Id)
            .Skip(pageInfo.SkipAmount)
            .Take(pageInfo.PageSize);

        // Retrieve
        TModel[] data = await query.ToArrayAsync(cancellationToken: ct);
        return new PaginatedData<TModel>(
            data,
            totalCount,
            pageInfo.PageNumber,
            (int)Math.Ceiling(totalCount / (double)pageInfo.PageSize)
        );
    }
}
