// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories.Data.User;
using InfiniLore.Server.Database.Models.Data.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Server.Database.Repositories.Data.User;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<ILoreScopeRepository>(ServiceLifetime.Scoped)]
public class LoreScopeRepository : UserDataRepository<LoreScope>, ILoreScopeRepository {

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async ValueTask<RepoResult> IsLoreScopeNameTakenAsync(string loreScopeName, Guid ownerId, CancellationToken ct = default) {
        if (loreScopeName.IsNullOrWhiteSpace()) return RepoResult.FromError(RepositoryFailures.ModelFailedValidation);
        if (ownerId == Guid.Empty) return RepoResult.FromError(RepositoryFailures.ModelFailedValidation);

        // Access
        DbSet<LoreScope> dbSet =  GetCachedDbSet<LoreScope>();

        // Query
        IQueryable<LoreScope> query = dbSet.Where(l =>
            l.Name == loreScopeName
            && l.OwnerId == ownerId
        );

        // Retrieve
        bool result = await query.AnyAsync(cancellationToken: ct);
        return RepoResult.FromState(result);

    }
    public async ValueTask<RepoResult> IsLoreScopeNameNotTakenAsync(string loreScopeName, Guid ownerId, CancellationToken ct = default) {
        if (loreScopeName.IsNullOrWhiteSpace()) return RepoResult.FromError(RepositoryFailures.ModelFailedValidation);
        if (ownerId == Guid.Empty) return RepoResult.FromError(RepositoryFailures.ModelFailedValidation);

        // Access
        DbSet<LoreScope> dbSet =  GetCachedDbSet<LoreScope>();

        // Query
        IQueryable<LoreScope> query = dbSet.Where(l =>
            l.Name == loreScopeName
            && l.OwnerId == ownerId
        );

        // Retrieve
        bool result = await query.AnyAsync(cancellationToken: ct);
        return RepoResult.FromState(!result);

    }
}
