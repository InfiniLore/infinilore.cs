// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories.Account;
using InfiniLore.Server.Database.Models.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Server.Database.Repositories.Account;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IUserRepository>(ServiceLifetime.Scoped)]
public class UserRepository : BasicDataRepository<InfiniLoreUser>, IUserRepository {

    #region ByAuth0Id
    private static IQueryable<InfiniLoreUser> ByAuth0IdQuery(IQueryable<InfiniLoreUser> query, string auth0Id) 
        => query.Where(ls => 
            (ls.Auth0IdGoogle != null && ls.Auth0IdGoogle == auth0Id)
            || (ls.Auth0Github != null && ls.Auth0Github == auth0Id )
        );

    public async ValueTask<RepoResult<InfiniLoreUser>> TryGetByAuth0IdAsync(string auth0Id, CancellationToken ct = default) {
        // Define Access
        DbSet<InfiniLoreUser> dbSet = GetCachedDbSet<InfiniLoreUser>();
        
        // Build Query
        IQueryable<InfiniLoreUser> query = dbSet.With(ByAuth0IdQuery, auth0Id);
        InfiniLoreUser? result = await query
            .SingleOrDefaultAsync(cancellationToken: ct);
        
        // Retrieve Data
        if (result is null) return RepoResult<InfiniLoreUser>.FromFailure(RepositoryFailures.ModelNotFound);
        return RepoResult<InfiniLoreUser>.FromSuccess(result);
    }
     
    public async ValueTask<RepoResult<InfiniLoreUser>> TryGetByAuth0IdWithAutoIncludeAsync(string auth0Id, CancellationToken ct = default) {
        // Define Access
        DbSet<InfiniLoreUser> dbSet = GetCachedDbSet<InfiniLoreUser>();
        
        // Build Query
        IQueryable<InfiniLoreUser> query = dbSet
            .With(ByAuth0IdQuery, auth0Id)
            .With(AutoInclude);
        InfiniLoreUser? result = await query.FirstOrDefaultAsync(cancellationToken: ct);
        
        // Retrieve Data
        if (result is null) return RepoResult<InfiniLoreUser>.FromFailure(RepositoryFailures.ModelNotFound);
        return RepoResult<InfiniLoreUser>.FromSuccess(result);
    }
    #endregion
}
