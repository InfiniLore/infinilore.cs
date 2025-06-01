// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Server.Modules.LoreScopes.Database;
using Microsoft.EntityFrameworkCore;

namespace InfiniLore.Modules.LoreScopes.Server.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<ILoreScopeRepository>]
public class LoreScopeRepository : OwnedModelRepository<InfiniLoreUserModel, LoreScopeModel>, ILoreScopeRepository {

    protected override IQueryable<LoreScopeModel> AlwaysInclude(IQueryable<LoreScopeModel> query) 
        => base.AlwaysInclude(query)
        .Include(l => l.PosterImageMetaData);

    protected override IQueryable<LoreScopeModel> OptionalInclude(IQueryable<LoreScopeModel> query) 
        => base.OptionalInclude(query)
        .Include(l => l.AccessProtection).ThenInclude(ap => ap!.Rules)
        .Include(l => l.PosterImageMetaData);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public ValueTask<Result> IsNameTakenAsync(string name, Guid ownerId, Guid notIncludedId = default, CancellationToken ct = default) 
        => CommonRepoMethods.IsNameTakenAsync(GetCachedDbSet<LoreScopeModel>(), name, ownerId, notIncludedId, ct);
    
    public ValueTask<Result> IsNameNotTakenAsync(string name, Guid ownerId, Guid notIncludedId = default, CancellationToken ct = default)
        => CommonRepoMethods.IsNameNotTakenAsync(GetCachedDbSet<LoreScopeModel>(),name, ownerId, notIncludedId, ct);
    
    public async ValueTask<Result> HasAccessPermissionAsync(Guid resourceId, Guid userId, string permission, CancellationToken ct = default) {
        if (resourceId == Guid.Empty || userId == Guid.Empty || permission.IsNullOrEmpty()) return Result.FromError(RepositoryFailures.ModelFailedValidation);
        
        DbSet<LoreScopeModel> dbSet = GetCachedDbSet<LoreScopeModel>();
        IQueryable<LoreScopeModel> query = dbSet
            .Where(l => 
                l.Id == resourceId 
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
