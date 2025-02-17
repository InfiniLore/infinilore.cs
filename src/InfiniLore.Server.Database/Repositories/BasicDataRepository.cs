// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories;
using InfiniLore.Server.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace InfiniLore.Server.Database.Repositories;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class BasicDataRepository<T> : UnitOfWorkRepository<ContentDb>, IBasicDataRepository<T> where T : BasicData {
    protected virtual IQueryable<T> AutoInclude(IQueryable<T> query) => query;
    
    protected virtual async ValueTask<bool> IsNotUniqueAsync(T originalModel, CancellationToken ct = default) {
        DbSet<T> dbSet = GetDbSet<T>();
        return await dbSet.AnyAsync(foundModel => foundModel.Id == originalModel.Id, ct);

    }
    protected virtual async ValueTask<bool> IsNotUniqueRangeAsync(T[] originalModels, CancellationToken ct = default) {
        DbSet<T> dbSet = GetDbSet<T>();
        
        Guid[] modelIds = originalModels.Select(m => m.Id).ToArray();
        
        return await dbSet.AnyAsync(foundModel => modelIds.Contains(foundModel.Id), ct);
        
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Repository Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async ValueTask<RepoResult> TryAddAsync(T model, CancellationToken ct = default) {
        // Define Access
        ContentDb dbContext = GetDbContext();
        DbSet<T> dbSet = GetDbSet<T>();
        
        if (await IsNotUniqueAsync(model, ct)) return RepoResult.FromFailure(RepositoryFailures.ModelFailedUniqueConstraint);
        model.UpdateLastModifiedDate();
        
        // Build Query
        await dbSet.AddAsync(model, cancellationToken: ct);
        await dbContext.SaveChangesAsync(ct);

        // Retrieve Data
        return RepoResult.Success;
    }
    
    public async ValueTask<RepoResult> TryAddRangeAsync(IEnumerable<T> models, CancellationToken ct = default) {
        // Define Access
        ContentDb dbContext = GetDbContext();
        DbSet<T> dbSet = GetDbSet<T>();
        
        T[] content = models as T[] ?? models.ToArray();
        if (await IsNotUniqueRangeAsync(content, ct)) return RepoResult.FromFailure(RepositoryFailures.ModelFailedUniqueConstraint);

        // Build Query
        foreach (T model in content) {
            model.UpdateLastModifiedDate();
        }
        await dbSet.AddRangeAsync(content, ct);
        await dbContext.SaveChangesAsync(ct);

        // Retrieve Data
        return RepoResult.Success;
    }
    
    public async ValueTask<RepoResult> TryUpdateAsync(T model, CancellationToken ct = default) {
        // Define Access
        ContentDb dbContext = GetDbContext();
        DbSet<T> dbSet = GetDbSet<T>();

        T? existing = await dbSet.FindAsync([model.Id], ct);
        if (existing is null) return RepoResult.FromFailure(RepositoryFailures.ModelNotFound);

        // Build Query
        model.UpdateLastModifiedDate();
        dbContext.Entry(existing).CurrentValues.SetValues(model);
        await dbContext.SaveChangesAsync(ct);
        
        // Retrieve Data
        return RepoResult.Success;
    }

    public async ValueTask<RepoResult> TryUpdateRangeAsync(IEnumerable<T> models, CancellationToken ct = default) {
        // Define Access
        ContentDb dbContext = GetDbContext();
        DbSet<T> dbSet = GetDbSet<T>();

        T[] modelArray = models as T[] ?? models.ToArray();
        Guid[] idsToUpdate = modelArray.Select(m => m.Id).ToArray();
        List<T> existingEntities = await dbSet.Where(m => idsToUpdate.Contains(m.Id)).ToListAsync(ct);
        if (existingEntities.Count != modelArray.Length) return RepoResult.FromFailure(RepositoryFailures.ModelNotFound);
        
        // Build Query
        foreach (T existingEntity in existingEntities) {
            T updatedModel = modelArray.First(m => m.Id == existingEntity.Id);
            existingEntity.UpdateLastModifiedDate();// Update individual properties
            dbContext.Entry(existingEntity).CurrentValues.SetValues(updatedModel);// Map the changes
        }
        await dbContext.SaveChangesAsync(ct);

        // Retrieve Data
        return RepoResult.Success;
    }

    public async ValueTask<RepoResult> TryAddOrUpdateAsync(T model, CancellationToken ct = default) {
        if (model.Id == Guid.Empty) return await TryAddAsync(model, ct);// If no ID, always add
        if (await IsNotUniqueAsync(model, ct)) return await TryUpdateAsync(model, ct); // If ID exists, update
        return await TryAddAsync(model, ct);
    }

    public async ValueTask<RepoResult> TryAddOrUpdateRangeAsync(IEnumerable<T> models, CancellationToken ct = default) {
        ContentDb dbContext = GetDbContext();
        DbSet<T> dbSet = GetDbSet<T>();
        
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

        return RepoResult.Success;
    }
    
    public async ValueTask<RepoResult> TryDeleteAsync(T model, CancellationToken ct = default) {
        // Define Access
        ContentDb dbContext = GetDbContext();
        DbSet<T> dbSet = GetDbSet<T>();
        
        T? existing = await dbSet.FindAsync([model.Id], ct);
        if (existing == null) return RepoResult.FromFailure(RepositoryFailures.ModelNotFound);

        // Build Query
        existing.SoftDelete();
        dbContext.Entry(existing).CurrentValues.SetValues(existing);
        await dbContext.SaveChangesAsync(ct);

        // Retrieve Data   
        return RepoResult.Success;
    }

    public async ValueTask<RepoResult> TryDeleteByIdAsync(Guid id, CancellationToken ct = default) {
        // Define Access
        ContentDb dbContext = GetDbContext();
        DbSet<T> dbSet = GetDbSet<T>();
        
        T? existing = await dbSet.FindAsync([id], ct);
        if (existing == null) return RepoResult.FromFailure(RepositoryFailures.ModelNotFound);

        // Build Query
        existing.SoftDelete();
        dbContext.Entry(existing).CurrentValues.SetValues(existing);
        await dbContext.SaveChangesAsync(ct);

        // Retrieve Data   
        return RepoResult.Success;
        
    }
    public async ValueTask<RepoResult> TryDeleteRangeAsync(IEnumerable<T> models, CancellationToken ct = default) {
        // Define Access
        DbSet<T> dbSet = GetDbSet<T>();
        
        Guid[] ids = models.Select(model => model.Id).ToArray();

        // Build Query
        await dbSet
            .Where(model => ids.Contains(model.Id))
            .ExecuteUpdateAsync(setPropertyCalls: s => BasicData.SoftDelete(s), ct);

        // Retrieve Data
        return RepoResult.Success;
    }
    
    public async ValueTask<RepoResult> TryDeleteRangeByIdAsync(IEnumerable<Guid> ids, CancellationToken ct = default) {
        // Define Access
        DbSet<T> dbSet = GetDbSet<T>();
        
        // Build Query
        await dbSet
            .Where(model => ids.Contains(model.Id))
            .ExecuteUpdateAsync(setPropertyCalls: s => BasicData.SoftDelete(s), ct);
        
        // Retrieve Data
        return RepoResult.Success;
    }
    public async ValueTask<RepoResult> TryRemoveAsync(T model, CancellationToken ct = default) {
        // Define Access
        ContentDb dbContext = GetDbContext();
        DbSet<T> dbSet = GetDbSet<T>();
        
        T? existing = await dbSet.FindAsync([model.Id], ct);
        if (existing == null) return RepoResult.FromFailure(RepositoryFailures.ModelNotFound);
        
        // Build Query
        dbSet.Remove(existing);
        await dbContext.SaveChangesAsync(ct);
        
        // Retrieve Data
        return RepoResult.Success;
    }
    
    public async ValueTask<RepoResult> TryRemoveByIdAsync(Guid id, CancellationToken ct = default) {
        // Define Access
        ContentDb dbContext = GetDbContext();
        DbSet<T> dbSet = GetDbSet<T>();
        
        T? existing = await dbSet.FindAsync([id], ct);
        if (existing == null) return RepoResult.FromFailure(RepositoryFailures.ModelNotFound);
        
        // Build Query
        dbSet.Remove(existing);
        await dbContext.SaveChangesAsync(ct);
        
        // Retrieve Data
        return RepoResult.Success;
    }

    public async ValueTask<RepoResult> TryRemoveRangeAsync(IEnumerable<T> models, CancellationToken ct = default) {
        // Define Access
        DbSet<T> dbSet = GetDbSet<T>();
        
        Guid[] ids = models.Select(model => model.Id).ToArray();
        
        // Build Query
        await dbSet
            .Where(model => ids.Contains(model.Id))
            .ExecuteDeleteAsync(ct);
        
        // Retrieve Data
        return RepoResult.Success;
    }
    
    public async ValueTask<RepoResult> TryRemoveRangeByIdAsync(IEnumerable<Guid> ids, CancellationToken ct = default) {
        // Define Access
        DbSet<T> dbSet = GetDbSet<T>();
        
        // Build Query
        await dbSet
            .Where(model => ids.Contains(model.Id))
            .ExecuteDeleteAsync(ct);
        
        // Retrieve Data
        return RepoResult.Success;
    }

    public async ValueTask<RepoResult<T>> TryGetByIdAsync(Guid id, CancellationToken ct = default) {
        // Define Access
        DbSet<T> dbSet = GetDbSet<T>();
        
        // Build Query
        T? result = await  dbSet.Where(ls => ls.Id == id)
            .FirstOrDefaultAsync(cancellationToken: ct);
        
        // Retrieve Data
        if (result is null) return RepoResult<T>.FromFailure(RepositoryFailures.ModelNotFound);
        return RepoResult<T>.FromSuccess(result);
    }
    
    public async ValueTask<RepoResult<T>> TryGetByIdWithAutoIncludeAsync(Guid id, CancellationToken ct = default) {
        // Define Access
        DbSet<T> dbSet = GetDbSet<T>();
        
        // Build Query
        IQueryable<T> query = dbSet.Where(ls => ls.Id == id);
        query = AutoInclude(query);
        T? result = await query.FirstOrDefaultAsync(cancellationToken: ct);
        
        // Retrieve Data
        if (result is null) return RepoResult<T>.FromFailure(RepositoryFailures.ModelNotFound);
        return RepoResult<T>.FromSuccess(result);
    }

    public async ValueTask<RepoResult<T[]>> TryGetAllAsync(CancellationToken ct = default) {
        // Define Access
        DbSet<T> dbSet = GetDbSet<T>();
        
        // Build Query & Retrieve Data
        T[] data = await dbSet.ToArrayAsync(cancellationToken: ct);
        return RepoResult<T[]>.FromSuccess(data);
    }

    public async ValueTask<RepoResult<T[]>> TryGetAllWithAutoIncludeAsync(CancellationToken ct = default) {
        // Define Access
        DbSet<T> dbSet = GetDbSet<T>();
        
        // Build Query
        IQueryable<T> query = dbSet;
        
        // Retrieve Data
        T[] data = await AutoInclude(query).ToArrayAsync(cancellationToken: ct);
        return RepoResult<T[]>.FromSuccess(data);
    }
    public async ValueTask<RepoResult<T[]>> TryGetAllReverseAsync(CancellationToken ct = default) {
        // Define Access
        DbSet<T> dbSet = GetDbSet<T>();
        
        // Build Query
        IQueryable<T> query = dbSet
            .OrderByDescending(ls => ls.Id)
            .Reverse();
        
        // Retrieve Data
        T[] data = await query.ToArrayAsync(cancellationToken: ct);
        return RepoResult<T[]>.FromSuccess(data);
        
    }
    
    public async ValueTask<RepoResult<T[]>> TryGetAllReverseWithAutoIncludeAsync(CancellationToken ct = default) {
        // Define Access
        DbSet<T> dbSet = GetDbSet<T>();
        
        // Build Query
        IQueryable<T> query = dbSet
            .OrderByDescending(ls => ls.Id)
            .Reverse();

        // Retrieve Data
        T[] data = await AutoInclude(query).ToArrayAsync(cancellationToken: ct);
        return RepoResult<T[]>.FromSuccess(data);
    }

    public async ValueTask<PaginatedRepoResult<T>> TryGetAllAsync(PaginationInfo pageInfo, CancellationToken ct = default) {
        // Define Access
        DbSet<T> dbSet = GetDbSet<T>();
        
        // Build Query
        int totalCount = await dbSet.CountAsync(ct);
        if (totalCount == 0) return PaginatedResult<T>.Empty;

        IQueryable<T> query = dbSet
            .OrderByDescending(ls => ls.Id)
            .Skip(pageInfo.SkipAmount)
            .Take(pageInfo.PageSize);
        
        // Retrieve Data
        T[] data = await query.ToArrayAsync(cancellationToken: ct);
        return new PaginatedResult<T>(
            Items: data,
            TotalCount: totalCount,
            CurrentPage: pageInfo.PageNumber,
            TotalPages: (int)Math.Ceiling(totalCount / (double)pageInfo.PageSize)
        );
    }
    public async ValueTask<PaginatedRepoResult<T>> TryGetAllWithAutoIncludeAsync(PaginationInfo pageInfo, CancellationToken ct = default) {
        // Define Access
        DbSet<T> dbSet = GetDbSet<T>();
        
        // Build Query
        int totalCount = await dbSet.CountAsync(ct);
        if (totalCount == 0) return PaginatedResult<T>.Empty;
        
        IQueryable<T> query = dbSet
            .OrderByDescending(ls => ls.Id)
            .Skip(pageInfo.SkipAmount)
            .Take(pageInfo.PageSize);
        
        // Retrieve Data
        T[] data = await AutoInclude(query).ToArrayAsync(cancellationToken: ct);
        return new PaginatedResult<T>(
            Items: data,
            TotalCount: totalCount,
            CurrentPage: pageInfo.PageNumber,
            TotalPages: (int)Math.Ceiling(totalCount / (double)pageInfo.PageSize)
        );
    }
    
    public async ValueTask<PaginatedRepoResult<T>> TryGetAllReverseAsync(PaginationInfo pageInfo, CancellationToken ct = default) {
        // Define Access
        DbSet<T> dbSet = GetDbSet<T>();
        
        // Build Query
        int totalCount = await dbSet.CountAsync(ct);
        if (totalCount == 0) return PaginatedResult<T>.Empty;

        IQueryable<T> query = dbSet
            .OrderByDescending(ls => ls.Id)
            .Reverse()
            .Skip(pageInfo.SkipAmount)
            .Take(pageInfo.PageSize);
        
        // Retrieve Data
        T[] data = await query.ToArrayAsync(cancellationToken: ct);
        return new PaginatedResult<T>(
            Items: data,
            TotalCount: totalCount,
            CurrentPage: pageInfo.PageNumber,
            TotalPages: (int)Math.Ceiling(totalCount / (double)pageInfo.PageSize)
        );
    }
    public async ValueTask<PaginatedRepoResult<T>> TryGetAllReverseWithAutoIncludeAsync(PaginationInfo pageInfo, CancellationToken ct = default) {
        // Define Access
        DbSet<T> dbSet = GetDbSet<T>();
        
        // Build Query
        int totalCount = await dbSet.CountAsync(ct);
        if (totalCount == 0) return PaginatedResult<T>.Empty;
        
        IQueryable<T> query = dbSet
            .OrderByDescending(ls => ls.Id)
            .Reverse()
            .Skip(pageInfo.SkipAmount)
            .Take(pageInfo.PageSize);
        
        // Retrieve Data
        T[] data = await AutoInclude(query).ToArrayAsync(cancellationToken: ct);
        return new PaginatedResult<T>(
            Items: data,
            TotalCount: totalCount,
            CurrentPage: pageInfo.PageNumber,
            TotalPages: (int)Math.Ceiling(totalCount / (double)pageInfo.PageSize)
        );
    }
    
    public async ValueTask<RepoResult<int>> TryCountAsync(CancellationToken ct = default) {
        // Define Access
        DbSet<T> dbSet = GetDbSet<T>();

        // Build Query & Retrieve Data
        int data = await dbSet.CountAsync(cancellationToken: ct);
        return RepoResult<int>.FromSuccess(data);
    }
}
