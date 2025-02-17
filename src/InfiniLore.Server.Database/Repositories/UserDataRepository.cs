// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories;
using InfiniLore.Server.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace InfiniLore.Server.Database.Repositories;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class UserDataRepository<T> : BasicDataRepository<T>, IUserDataRepository<T> where T : UserData {
    public async ValueTask<RepoResult<T[]>> TryGetByUserAsync(Guid userId, CancellationToken ct = default) {
        // Define Access
        DbSet<T> dbSet = GetDbSet<T>();
        
        // Build Query
        IQueryable<T> query = dbSet.Where(ls => ls.OwnerId == userId);
        
        // Retrieve Data
        T[] result = await query.ToArrayAsync(cancellationToken: ct);
        return RepoResult<T[]>.FromSuccess(result);
        
    }
    
    public async ValueTask<RepoResult<T[]>> TryGetByUserWithAutoIncludeAsync(Guid userId, CancellationToken ct = default) {
        // Define Access
        DbSet<T> dbSet = GetDbSet<T>();
        
        // Build Query
        IQueryable<T> query = dbSet.Where(ls => ls.OwnerId == userId);
        
        // Retrieve Data
        T[] result = await AutoInclude(query).ToArrayAsync(cancellationToken: ct);
        return RepoResult<T[]>.FromSuccess(result);
    }

    public async ValueTask<RepoResult<T[]>> TryGetByUserReverseAsync(Guid userId, CancellationToken ct = default) {
        // Define Access
        DbSet<T> dbSet = GetDbSet<T>();
        
        // Build Query
        IQueryable<T> query = dbSet
            .OrderByDescending(ls => ls.Id)
            .Reverse()
            .Where(ls => ls.OwnerId == userId);
        
        // Retrieve Data
        T[] result = await query.ToArrayAsync(cancellationToken: ct);
        return RepoResult<T[]>.FromSuccess(result);
    }
    
    public async ValueTask<RepoResult<T[]>> TryGetByUserReverseWithAutoIncludeAsync(Guid userId, CancellationToken ct = default) {
        // Define Access
        DbSet<T> dbSet = GetDbSet<T>();
        
        // Build Query
        IQueryable<T> query = dbSet
            .OrderByDescending(ls => ls.Id)
            .Reverse()
            .Where(ls => ls.OwnerId == userId);
        
        // Retrieve Data
        T[] result = await AutoInclude(query).ToArrayAsync(cancellationToken: ct);
        return RepoResult<T[]>.FromSuccess(result);
    }

    public async ValueTask<PaginatedRepoResult<T>> TryGetByUserAsync(Guid userId, PaginationInfo pageInfo, CancellationToken ct = default) {
        // Define Access
        DbSet<T> dbSet = GetDbSet<T>();
        
        // Build Query
        int totalCount = await dbSet.CountAsync(ct);
        if (totalCount == 0) return PaginatedResult<T>.Empty;

        IQueryable<T> query = dbSet
            .OrderByDescending(ls => ls.Id)
            .Where(ls => ls.OwnerId == userId)
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
    public async ValueTask<PaginatedRepoResult<T>> TryGetByUserWithAutoIncludeAsync(Guid userId, PaginationInfo pageInfo, CancellationToken ct = default)  {
        // Define Access
        DbSet<T> dbSet = GetDbSet<T>();
        
        // Build Query
        int totalCount = await dbSet.CountAsync(ct);
        if (totalCount == 0) return PaginatedResult<T>.Empty;
        
        IQueryable<T> query = dbSet
            .OrderByDescending(ls => ls.Id)
            .Where(ls => ls.OwnerId == userId)
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
    public async ValueTask<PaginatedRepoResult<T>> TryGetByUserReverseAsync(Guid userId, PaginationInfo pageInfo, CancellationToken ct = default)  {
        // Define Access
        DbSet<T> dbSet = GetDbSet<T>();
        
        // Build Query
        int totalCount = await dbSet.CountAsync(ct);
        if (totalCount == 0) return PaginatedResult<T>.Empty;

        IQueryable<T> query = dbSet
            .OrderByDescending(ls => ls.Id)
            .Where(ls => ls.OwnerId == userId)
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
    
    public async ValueTask<PaginatedRepoResult<T>> TryGetByUserReverseWithAutoIncludeAsync(Guid userId, PaginationInfo pageInfo, CancellationToken ct = default) {
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
}
