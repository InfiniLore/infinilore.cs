// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Server.Modules.Core.Database;
using Microsoft.EntityFrameworkCore;

namespace InfiniLore.Server.Modules.LoreScopes.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<ILoreScopeRepository>]
public class LoreScopeRepository : OwnedModelRepository<InfiniLoreUserModel, LoreScopeModel>, ILoreScopeRepository {

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async ValueTask<Result> IsLoreScopeNameTakenAsync(string loreScopeName, Guid ownerId, Guid notIncludedId = default, CancellationToken ct = default) {
        if (loreScopeName.IsNullOrWhiteSpace() || ownerId == Guid.Empty) return Result.FromError(RepositoryFailures.ModelFailedValidation);

        // Access
        DbSet<LoreScopeModel> dbSet = GetCachedDbSet<LoreScopeModel>();

        // Query
        IQueryable<LoreScopeModel> query = dbSet.Where(l =>
            l.Name == loreScopeName
            && l.OwnerId == ownerId
            && l.Id != notIncludedId
        );

        // Retrieve
        bool result = await query.AnyAsync(cancellationToken: ct);
        return Result.FromState(result);
    }
    
    public async ValueTask<Result> IsLoreScopeNameNotTakenAsync(string loreScopeName, Guid ownerId, Guid notIncludedId = default, CancellationToken ct = default) {
        if (loreScopeName.IsNullOrWhiteSpace() || ownerId == Guid.Empty) return Result.FromError(RepositoryFailures.ModelFailedValidation);

        // Access
        DbSet<LoreScopeModel> dbSet = GetCachedDbSet<LoreScopeModel>();

        // Query
        IQueryable<LoreScopeModel> query = dbSet.Where(l =>
            l.Name == loreScopeName
            && l.OwnerId == ownerId
            && l.Id != notIncludedId
        );

        // Retrieve
        bool result = await query.AnyAsync(cancellationToken: ct);
        return Result.FromState(!result);
    }
    
    public async ValueTask<Result> HasAccessPermissionAsync(Guid loreScopeId, Guid userId, string permission, CancellationToken ct = default) {
        if (loreScopeId == Guid.Empty || userId == Guid.Empty || permission.IsNullOrEmpty()) return Result.FromError(RepositoryFailures.ModelFailedValidation);
        
        DbSet<LoreScopeModel> dbSet = GetCachedDbSet<LoreScopeModel>();
        IQueryable<LoreScopeModel> query = dbSet
            .Where(l => 
                l.Id == loreScopeId 
                && (
                    l.OwnerId == userId 
                    || l.AccessProtectionId != null 
                    && l.AccessProtection!.Rules!.Any(r => r.UserId == userId && r.Permission == permission)
                )
            );
        
        bool exists = await query.AnyAsync(ct);

        return Result.FromState(exists);
    }
}
