// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Server.Contracts;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories;
using InfiniLore.Server.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace InfiniLore.Server.Database.Repositories;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class BasicDataRepository<T> : UnitOfWorkRepository<ContentDb>, IBasicDataRepository<T> where T : BasicData {

    public async ValueTask<Result<T>> GetByIdAsync(Guid id, QueryConfig config = default, CancellationToken ct = default) {
        // Access
        DbSet<T> dbSet = GetCachedDbSet<T>();

        // Query
        T? result = await dbSet
            .ConditionalWith(config.AutoInclude, AutoInclude)
            .Where(ls => ls.Id == id)
            .FirstOrDefaultAsync(cancellationToken: ct);

        // Retrieve
        if (result is null) return Result<T>.FromError(RepositoryFailures.ModelNotFound);

        return Result<T>.FromSuccess(result);
    }

    public async ValueTask<Result<T[]>> GetAllAsync(QueryConfig config = default, CancellationToken ct = default) {
        // Access
        DbSet<T> dbSet = GetCachedDbSet<T>();

        // Query
        IOrderedQueryable<T>? query = dbSet
            .ConditionalWith(config.AutoInclude, AutoInclude)
            .ConditionalReverse(config.Reverse)
            .OrderByDescending(ls => ls.Id);

        // Query & Retrieve
        T[] data = await query.ToArrayAsync(cancellationToken: ct);
        return Result<T[]>.FromSuccess(data);
    }

    public async ValueTask<PaginatedResult<T>> GetAllAsync(PaginationInfo pageInfo, QueryConfig config = default, CancellationToken ct = default) {
        // Access
        DbSet<T> dbSet = GetCachedDbSet<T>();

        // Query
        int totalCount = await dbSet.CountAsync(ct);
        if (totalCount == 0) return PaginatedData<T>.Empty;

        IQueryable<T> query = dbSet
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
    /// <summary>
    /// Counts the amount of <typeparamref name="T"/>s in the database.
    /// </summary>
    /// <returns>The amount of <typeparamref name="T"/>s in the database.</returns>
    public async ValueTask<Result<int>> GetCountAsync(CancellationToken ct = default) {
        // Access
        DbSet<T> dbSet = GetCachedDbSet<T>();

        // Query & Retrieve
        int data = await dbSet
            .AsNoTracking()
            .CountAsync(cancellationToken: ct);

        return Result<int>.FromSuccess(data);
    }

    public async ValueTask<Result> IsIdTakenAsync(Guid id, CancellationToken ct = default) {
        // Access
        DbSet<T> dbSet = GetCachedDbSet<T>();

        // Query & Retrieve
        bool result = await dbSet
            .AsNoTracking()
            .AnyAsync(predicate: ls => ls.Id == id, ct);

        return Result.FromState(result);
    }

    public async ValueTask<Result> IsIdNotTakenAsync(Guid id, CancellationToken ct = default) {
        // Access
        DbSet<T> dbSet = GetCachedDbSet<T>();

        // Query & Retrieve
        bool result = await dbSet
            .AsNoTracking()
            .AnyAsync(predicate: ls => ls.Id == id, ct);

        return Result.FromState(!result);
    }

    protected virtual IQueryable<T> AutoInclude(IQueryable<T> query) => query;

    protected virtual async ValueTask<bool> IsNotUniqueAsync(T[] modelsToValidate, CancellationToken ct = default) {
        DbSet<T> dbSet = GetCachedDbSet<T>();
        Guid[] ids = modelsToValidate.Select(m => m.Id).ToArray();
        bool result = await dbSet.AsNoTracking().AnyAsync(predicate: foundModel => ids.Contains(foundModel.Id), ct);
        return result;
    }

    private ValueTask<bool> IsNotUniqueAsync(T originalModel, CancellationToken ct = default)
        => IsNotUniqueAsync([originalModel], ct);

    // -----------------------------------------------------------------------------------------------------------------
    // Repository Methods
    // -----------------------------------------------------------------------------------------------------------------
    #region CRUD Operations
    public async ValueTask<Result> AddAsync(T model, CancellationToken ct = default) {
        // Access
        ContentDb dbContext = GetDbContext();
        DbSet<T> dbSet = GetCachedDbSet<T>();

        if (await IsNotUniqueAsync(model, ct)) return Result.FromError(RepositoryFailures.ModelFailedUniqueConstraint);

        model.UpdateLastModifiedDate();

        // Query
        await dbSet.AddAsync(model, ct);
        await dbContext.SaveChangesAsync(ct);

        // Retrieve
        return true;
    }

    public async ValueTask<Result> AddRangeAsync(IEnumerable<T> models, CancellationToken ct = default) {
        // Access
        ContentDb dbContext = GetDbContext();
        DbSet<T> dbSet = GetCachedDbSet<T>();

        T[] content = models as T[] ?? models.ToArray();
        if (await IsNotUniqueAsync(content, ct)) return Result.FromError(RepositoryFailures.ModelFailedUniqueConstraint);

        // Query
        foreach (T model in content) {
            model.UpdateLastModifiedDate();
        }

        await dbSet.AddRangeAsync(content, ct);
        await dbContext.SaveChangesAsync(ct);

        // Retrieve
        return true;
    }

    public async ValueTask<Result> UpdateAsync(T model, CancellationToken ct = default) {
        // Access
        ContentDb dbContext = GetDbContext();
        DbSet<T> dbSet = GetCachedDbSet<T>();

        T? existing = await dbSet.FindAsync([model.Id], ct);
        if (existing is null) return Result.FromError(RepositoryFailures.ModelNotFound);

        // Query
        model.UpdateLastModifiedDate();
        dbContext.Entry(existing).CurrentValues.SetValues(model);
        await dbContext.SaveChangesAsync(ct);

        // Retrieve
        return true;
    }

    public async ValueTask<Result> UpdateRangeAsync(IEnumerable<T> models, CancellationToken ct = default) {
        // Access
        ContentDb dbContext = GetDbContext();
        DbSet<T> dbSet = GetCachedDbSet<T>();

        T[] modelArray = models as T[] ?? models.ToArray();
        Guid[] idsToUpdate = modelArray.Select(m => m.Id).ToArray();
        T[] existingEntities = await dbSet.Where(m => idsToUpdate.Contains(m.Id)).ToArrayAsync(ct);
        if (existingEntities.Length != modelArray.Length) return Result.FromError(RepositoryFailures.ModelNotFound);

        // Query
        foreach (T existingEntity in existingEntities) {
            T updatedModel = modelArray.First(m => m.Id == existingEntity.Id);
            existingEntity.UpdateLastModifiedDate();// Update individual properties
            dbContext.Entry(existingEntity).CurrentValues.SetValues(updatedModel);// Map the changes
        }

        await dbContext.SaveChangesAsync(ct);

        // Retrieve
        return true;
    }

    public async ValueTask<Result> AddOrUpdateAsync(T model, CancellationToken ct = default) {
        if (model.Id == Guid.Empty) return await AddAsync(model, ct);// If no ID, always add
        if (await IsNotUniqueAsync(model, ct)) return await UpdateAsync(model, ct);// If ID exists, update

        return await AddAsync(model, ct);
    }

    public async ValueTask<Result> AddOrUpdateRangeAsync(IEnumerable<T> models, CancellationToken ct = default) {
        ContentDb dbContext = GetDbContext();
        DbSet<T> dbSet = GetCachedDbSet<T>();

        T[] userContents = models as T[] ?? models.ToArray();
        Guid[] modelIds = userContents.Select(m => m.Id).ToArray();

        // Fetch all existing models from the database
        List<T> existingModels = await dbSet.Where(m => modelIds.Contains(m.Id)).ToListAsync(ct);
        Guid[] existingModelIds = existingModels.Select(m => m.Id).ToArray();

        // Separate models into new and updateable ones
        IEnumerable<T> modelsToUpdate = userContents.Where(m => existingModelIds.Contains(m.Id));
        IEnumerable<T> modelsToAdd = userContents.Where(m => !existingModelIds.Contains(m.Id));

        // Handle tracked updates for existing models
        foreach (T modelToUpdate in modelsToUpdate) {
            T existingModel = existingModels.First(em => em.Id == modelToUpdate.Id);
            existingModel.UpdateLastModifiedDate();// Update required fields
            dbContext.Entry(existingModel).CurrentValues.SetValues(modelToUpdate);// Map incoming changes to tracked entity
        }

        // Add new models
        await dbSet.AddRangeAsync(modelsToAdd, ct);

        // Save all changes
        await dbContext.SaveChangesAsync(ct);

        return true;
    }

    public async ValueTask<Result> DeleteAsync(T model, CancellationToken ct = default) {
        // Access
        ContentDb dbContext = GetDbContext();
        DbSet<T> dbSet = GetCachedDbSet<T>();

        T? existing = await dbSet.FindAsync([model.Id], ct);
        if (existing == null) return Result.FromError(RepositoryFailures.ModelNotFound);

        // Query
        existing.SoftDelete();
        dbContext.Entry(existing).CurrentValues.SetValues(existing);
        await dbContext.SaveChangesAsync(ct);

        // Retrieve   
        return true;
    }

    public async ValueTask<Result> DeleteByIdAsync(Guid id, CancellationToken ct = default) {
        // Access
        ContentDb dbContext = GetDbContext();
        DbSet<T> dbSet = GetCachedDbSet<T>();

        T? existing = await dbSet.FindAsync([id], ct);
        if (existing == null) return Result.FromError(RepositoryFailures.ModelNotFound);

        // Query
        existing.SoftDelete();
        dbContext.Entry(existing).CurrentValues.SetValues(existing);
        await dbContext.SaveChangesAsync(ct);

        // Retrieve   
        return true;

    }

    public async ValueTask<Result> DeleteRangeAsync(IEnumerable<T> models, CancellationToken ct = default) {
        // Access
        DbSet<T> dbSet = GetCachedDbSet<T>();

        Guid[] ids = models.Select(model => model.Id).ToArray();

        // Query
        await dbSet
            .Where(model => ids.Contains(model.Id))
            .ExecuteUpdateAsync(setPropertyCalls: s => BasicData.SoftDelete(s), ct);

        // Retrieve
        return true;
    }

    public async ValueTask<Result> DeleteRangeByIdAsync(IEnumerable<Guid> ids, CancellationToken ct = default) {
        // Access
        DbSet<T> dbSet = GetCachedDbSet<T>();

        // Query
        await dbSet
            .Where(model => ids.Contains(model.Id))
            .ExecuteUpdateAsync(setPropertyCalls: s => BasicData.SoftDelete(s), ct);

        // Retrieve
        return true;
    }

    public async ValueTask<Result> RemoveAsync(T model, CancellationToken ct = default) {
        // Access
        ContentDb dbContext = GetDbContext();
        DbSet<T> dbSet = GetCachedDbSet<T>();

        T? existing = await dbSet.FindAsync([model.Id], ct);
        if (existing == null) return Result.FromError(RepositoryFailures.ModelNotFound);

        // Query
        dbSet.Remove(existing);
        await dbContext.SaveChangesAsync(ct);

        // Retrieve
        return true;
    }

    public async ValueTask<Result> RemoveByIdAsync(Guid id, CancellationToken ct = default) {
        // Access
        ContentDb dbContext = GetDbContext();
        DbSet<T> dbSet = GetCachedDbSet<T>();

        T? existing = await dbSet.FindAsync([id], ct);
        if (existing == null) return Result.FromError(RepositoryFailures.ModelNotFound);

        // Query
        dbSet.Remove(existing);
        await dbContext.SaveChangesAsync(ct);

        // Retrieve
        return true;
    }

    public async ValueTask<Result> RemoveRangeAsync(IEnumerable<T> models, CancellationToken ct = default) {
        // Access
        DbSet<T> dbSet = GetCachedDbSet<T>();

        Guid[] ids = models.Select(model => model.Id).ToArray();

        // Query
        await dbSet
            .Where(model => ids.Contains(model.Id))
            .ExecuteDeleteAsync(ct);

        // Retrieve
        return true;
    }

    public async ValueTask<Result> RemoveRangeByIdAsync(IEnumerable<Guid> ids, CancellationToken ct = default) {
        // Access
        DbSet<T> dbSet = GetCachedDbSet<T>();

        // Query
        await dbSet
            .Where(model => ids.Contains(model.Id))
            .ExecuteDeleteAsync(ct);

        // Retrieve
        return true;
    }
    #endregion
}
