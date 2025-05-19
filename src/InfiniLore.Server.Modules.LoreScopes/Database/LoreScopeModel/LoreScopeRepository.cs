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
    public async ValueTask<Result> IsLoreScopeNameTakenAsync(string loreScopeName, Guid ownerId, CancellationToken ct = default) {
        if (loreScopeName.IsNullOrWhiteSpace() || ownerId == Guid.Empty) return Result.FromError(RepositoryFailures.ModelFailedValidation);

        // Access
        DbSet<LoreScopeModel> dbSet = GetCachedDbSet<LoreScopeModel>();

        // Query
        IQueryable<LoreScopeModel> query = dbSet.Where(l =>
            l.Name == loreScopeName
            && l.OwnerId == ownerId
        );

        // Retrieve
        bool result = await query.AnyAsync(cancellationToken: ct);
        return Result.FromState(result);

    }
    public async ValueTask<Result> IsLoreScopeNameNotTakenAsync(string loreScopeName, Guid ownerId, CancellationToken ct = default) {
        if (loreScopeName.IsNullOrWhiteSpace() || ownerId == Guid.Empty) return Result.FromError(RepositoryFailures.ModelFailedValidation);

        // Access
        DbSet<LoreScopeModel> dbSet = GetCachedDbSet<LoreScopeModel>();

        // Query
        IQueryable<LoreScopeModel> query = dbSet.Where(l =>
            l.Name == loreScopeName
            && l.OwnerId == ownerId
        );

        // Retrieve
        bool result = await query.AnyAsync(cancellationToken: ct);
        return Result.FromState(!result);
    }
}
