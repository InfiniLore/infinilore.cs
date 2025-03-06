// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
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
    public async ValueTask<RepoResult> AddAsync(T model, CancellationToken ct = default) {
        // Access
        ContentDb dbContext = GetDbContext();
        DbSet<T> dbSet = GetCachedDbSet<T>();

        if (await IsNotUniqueAsync(model, ct)) return RepoResult.FromError(RepositoryFailures.ModelFailedUniqueConstraint);

        model.UpdateLastModifiedDate();

        // Query
        await dbSet.AddAsync(model, ct);
        await dbContext.SaveChangesAsync(ct);

        // Retrieve
        return true;
    }

    public async ValueTask<RepoResult> AddRangeAsync(IEnumerable<T> models, CancellationToken ct = default) {
        // Access
        ContentDb dbContext = GetDbContext();
        DbSet<T> dbSet = GetCachedDbSet<T>();

        T[] content = models as T[] ?? models.ToArray();
        if (await IsNotUniqueAsync(content, ct)) return RepoResult.FromError(RepositoryFailures.ModelFailedUniqueConstraint);

        // Query
        foreach (T model in content) {
            model.UpdateLastModifiedDate();
        }

        await dbSet.AddRangeAsync(content, ct);
        await dbContext.SaveChangesAsync(ct);

        // Retrieve
        return true;
    }

    public async ValueTask<RepoResult> UpdateAsync(T model, CancellationToken ct = default) {
        // Access
        ContentDb dbContext = GetDbContext();
        DbSet<T> dbSet = GetCachedDbSet<T>();

        T? existing = await dbSet.FindAsync([model.Id], ct);
        if (existing is null) return RepoResult.FromError(RepositoryFailures.ModelNotFound);

        // Query
        model.UpdateLastModifiedDate();
        dbContext.Entry(existing).CurrentValues.SetValues(model);
        await dbContext.SaveChangesAsync(ct);

        // Retrieve
        return true;
    }

    public async ValueTask<RepoResult> UpdateRangeAsync(IEnumerable<T> models, CancellationToken ct = default) {
        // Access
        ContentDb dbContext = GetDbContext();
        DbSet<T> dbSet = GetCachedDbSet<T>();

        T[] modelArray = models as T[] ?? models.ToArray();
        Guid[] idsToUpdate = modelArray.Select(m => m.Id).ToArray();
        T[] existingEntities = await dbSet.Where(m => idsToUpdate.Contains(m.Id)).ToArrayAsync(ct);
        if (existingEntities.Length != modelArray.Length) return RepoResult.FromError(RepositoryFailures.ModelNotFound);

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

    public async ValueTask<RepoResult> AddOrUpdateAsync(T model, CancellationToken ct = default) {
        if (model.Id == Guid.Empty) return await AddAsync(model, ct);// If no ID, always add
        if (await IsNotUniqueAsync(model, ct)) return await UpdateAsync(model, ct);// If ID exists, update

        return await AddAsync(model, ct);
    }

    public async ValueTask<RepoResult> AddOrUpdateRangeAsync(IEnumerable<T> models, CancellationToken ct = default) {
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

    public async ValueTask<RepoResult> DeleteAsync(T model, CancellationToken ct = default) {
        // Access
        ContentDb dbContext = GetDbContext();
        DbSet<T> dbSet = GetCachedDbSet<T>();

        T? existing = await dbSet.FindAsync([model.Id], ct);
        if (existing == null) return RepoResult.FromError(RepositoryFailures.ModelNotFound);

        // Query
        existing.SoftDelete();
        dbContext.Entry(existing).CurrentValues.SetValues(existing);
        await dbContext.SaveChangesAsync(ct);

        // Retrieve   
        return true;
    }

    public async ValueTask<RepoResult> DeleteByIdAsync(Guid id, CancellationToken ct = default) {
        // Access
        ContentDb dbContext = GetDbContext();
        DbSet<T> dbSet = GetCachedDbSet<T>();

        T? existing = await dbSet.FindAsync([id], ct);
        if (existing == null) return RepoResult.FromError(RepositoryFailures.ModelNotFound);

        // Query
        existing.SoftDelete();
        dbContext.Entry(existing).CurrentValues.SetValues(existing);
        await dbContext.SaveChangesAsync(ct);

        // Retrieve   
        return true;

    }
    public async ValueTask<RepoResult> DeleteRangeAsync(IEnumerable<T> models, CancellationToken ct = default) {
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

    public async ValueTask<RepoResult> DeleteRangeByIdAsync(IEnumerable<Guid> ids, CancellationToken ct = default) {
        // Access
        DbSet<T> dbSet = GetCachedDbSet<T>();

        // Query
        await dbSet
            .Where(model => ids.Contains(model.Id))
            .ExecuteUpdateAsync(setPropertyCalls: s => BasicData.SoftDelete(s), ct);

        // Retrieve
        return true;
    }
    public async ValueTask<RepoResult> RemoveAsync(T model, CancellationToken ct = default) {
        // Access
        ContentDb dbContext = GetDbContext();
        DbSet<T> dbSet = GetCachedDbSet<T>();

        T? existing = await dbSet.FindAsync([model.Id], ct);
        if (existing == null) return RepoResult.FromError(RepositoryFailures.ModelNotFound);

        // Query
        dbSet.Remove(existing);
        await dbContext.SaveChangesAsync(ct);

        // Retrieve
        return true;
    }

    public async ValueTask<RepoResult> RemoveByIdAsync(Guid id, CancellationToken ct = default) {
        // Access
        ContentDb dbContext = GetDbContext();
        DbSet<T> dbSet = GetCachedDbSet<T>();

        T? existing = await dbSet.FindAsync([id], ct);
        if (existing == null) return RepoResult.FromError(RepositoryFailures.ModelNotFound);

        // Query
        dbSet.Remove(existing);
        await dbContext.SaveChangesAsync(ct);

        // Retrieve
        return true;
    }

    public async ValueTask<RepoResult> RemoveRangeAsync(IEnumerable<T> models, CancellationToken ct = default) {
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

    public async ValueTask<RepoResult> RemoveRangeByIdAsync(IEnumerable<Guid> ids, CancellationToken ct = default) {
        // Access
        DbSet<T> dbSet = GetCachedDbSet<T>();

        // Query
        await dbSet
            .Where(model => ids.Contains(model.Id))
            .ExecuteDeleteAsync(ct);

        // Retrieve
        return true;
    }

    public async ValueTask<RepoResult<T>> GetByIdAsync(Guid id, CancellationToken ct = default) {
        // Access
        DbSet<T> dbSet = GetCachedDbSet<T>();

        // Query
        T? result = await dbSet.Where(ls => ls.Id == id)
            .FirstOrDefaultAsync(cancellationToken: ct);

        // Retrieve
        if (result is null) return RepoResult<T>.FromError(RepositoryFailures.ModelNotFound);
        return RepoResult<T>.FromSuccess(result);
    }

    public async ValueTask<RepoResult<T>> GetByIdWithAutoIncludeAsync(Guid id, CancellationToken ct = default) {
        // Access
        DbSet<T> dbSet = GetCachedDbSet<T>();

        // Query
        IQueryable<T> query = dbSet
            .Where(ls => ls.Id == id)
            .With(AutoInclude);

        // Retrieve
        T? result = await query.FirstOrDefaultAsync(cancellationToken: ct);
        return result is not null 
            ? RepoResult<T>.FromSuccess(result)
            : RepoResult<T>.FromError(RepositoryFailures.ModelNotFound);
    }

    public async ValueTask<RepoResult<T[]>> GetAllAsync(CancellationToken ct = default) {
        // Access
        DbSet<T> dbSet = GetCachedDbSet<T>();

        // Query & Retrieve
        T[] data = await dbSet.ToArrayAsync(cancellationToken: ct);
        return RepoResult<T[]>.FromSuccess(data);
    }

    public async ValueTask<RepoResult<T[]>> GetAllWithAutoIncludeAsync(CancellationToken ct = default) {
        // Access
        DbSet<T> dbSet = GetCachedDbSet<T>();

        // Query
        IQueryable<T> query = dbSet
            .With(AutoInclude);

        // Retrieve
        T[] data = await query.ToArrayAsync(cancellationToken: ct);
        return RepoResult<T[]>.FromSuccess(data);
    }
    public async ValueTask<RepoResult<T[]>> GetAllReverseAsync(CancellationToken ct = default) {
        // Access
        DbSet<T> dbSet = GetCachedDbSet<T>();

        // Query
        IQueryable<T> query = dbSet
            .OrderByDescending(ls => ls.Id)
            .Reverse();

        // Retrieve
        T[] data = await query.ToArrayAsync(cancellationToken: ct);
        return RepoResult<T[]>.FromSuccess(data);

    }

    public async ValueTask<RepoResult<T[]>> GetAllReverseWithAutoIncludeAsync(CancellationToken ct = default) {
        // Access
        DbSet<T> dbSet = GetCachedDbSet<T>();

        // Query
        IQueryable<T> query = dbSet
            .OrderByDescending(ls => ls.Id)
            .With(AutoInclude)
            .Reverse();

        // Retrieve
        T[] data = await query.ToArrayAsync(cancellationToken: ct);
        return RepoResult<T[]>.FromSuccess(data);
    }

    public async ValueTask<PaginatedRepoResult<T>> GetAllAsync(PaginationInfo pageInfo, CancellationToken ct = default) {
        // Access
        DbSet<T> dbSet = GetCachedDbSet<T>();

        // Query
        int totalCount = await dbSet.CountAsync(ct);
        if (totalCount == 0) return PaginatedResult<T>.Empty;

        IQueryable<T> query = dbSet
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
    public async ValueTask<PaginatedRepoResult<T>> GetAllWithAutoIncludeAsync(PaginationInfo pageInfo, CancellationToken ct = default) {
        // Access
        DbSet<T> dbSet = GetCachedDbSet<T>();

        // Query
        int totalCount = await dbSet.CountAsync(ct);
        if (totalCount == 0) return PaginatedResult<T>.Empty;

        IQueryable<T> query = dbSet
            .OrderByDescending(ls => ls.Id)
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

    public async ValueTask<PaginatedRepoResult<T>> GetAllReverseAsync(PaginationInfo pageInfo, CancellationToken ct = default) {
        // Access
        DbSet<T> dbSet = GetCachedDbSet<T>();

        // Query
        int totalCount = await dbSet.CountAsync(ct);
        if (totalCount == 0) return PaginatedResult<T>.Empty;

        IQueryable<T> query = dbSet
            .OrderByDescending(ls => ls.Id)
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
    public async ValueTask<PaginatedRepoResult<T>> GetAllReverseWithAutoIncludeAsync(PaginationInfo pageInfo, CancellationToken ct = default) {
        // Access
        DbSet<T> dbSet = GetCachedDbSet<T>();

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

    public async ValueTask<RepoResult<int>> GetCountAsync(CancellationToken ct = default) {
        // Access
        DbSet<T> dbSet = GetCachedDbSet<T>();

        // Query & Retrieve
        int data = await dbSet.AsNoTracking()
            .CountAsync(cancellationToken: ct);
        return RepoResult<int>.FromSuccess(data);
    }

    public async ValueTask<RepoResult> IsIdTakenAsync(Guid id, CancellationToken ct = default) {
        // Access
        DbSet<T> dbSet = GetCachedDbSet<T>();

        // Query & Retrieve
        bool result = await dbSet.AsNoTracking()
            .AnyAsync(ls => ls.Id == id, ct);
        
        return RepoResult.FromState(result);
    }

    public async ValueTask<RepoResult> IsIdNotTakenAsync(Guid id, CancellationToken ct = default) 
        => !await IsIdTakenAsync(id, ct);
}
