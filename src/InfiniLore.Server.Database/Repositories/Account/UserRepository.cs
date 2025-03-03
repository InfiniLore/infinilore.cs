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

    public async ValueTask<RepoResult<Guid>> TryGetIdByAuth0IdAsync(string auth0Id, CancellationToken ct = default) {
        if (auth0Id.IsNullOrWhiteSpace()) return RepoResult<Guid>.FromFailure(RepositoryFailures.ModelFailedValidation);
        
        // Access
        DbSet<InfiniLoreUser> dbSet = GetCachedDbSet<InfiniLoreUser>();

        // Query
        IQueryable<Guid> query = dbSet.AsNoTracking()
            .With(ByAuth0IdQuery, auth0Id)
            .Select(ls => ls.Id);

        // Retrieve
        Guid result = await query.FirstOrDefaultAsync(cancellationToken: ct);
        if (result == Guid.Empty) return RepoResult<Guid>.FromFailure(RepositoryFailures.ModelNotFound);
        return RepoResult<Guid>.FromSuccess(result);
    }
    
    public async ValueTask<RepoResult> IsUsernameTakenAsync(string username, Guid skipUserId = default, CancellationToken ct = default) {
        if (username.IsNullOrWhiteSpace()) return RepoResult.FromFailure(RepositoryFailures.ModelFailedValidation);

        // Access
        DbSet<InfiniLoreUser> dbSet = GetCachedDbSet<InfiniLoreUser>();

        // Query
        IQueryable<InfiniLoreUser> query = dbSet
            .Where(ls => ls.Username == username)
            .ConditionalWhere(
                skipUserId != Guid.Empty,
                predicate: ls => ls.Id != skipUserId
            );

        // Retrieve
        bool result = await query.AnyAsync(cancellationToken: ct);
        return RepoResult.FromBool(result);
    }

    public async ValueTask<RepoResult> IsUsernameNotTakenAsync(string username, Guid skipUserId = default, CancellationToken ct = default) {
        if (username.IsNullOrWhiteSpace()) return RepoResult.FromFailure(RepositoryFailures.ModelFailedValidation);

        // Access
        DbSet<InfiniLoreUser> dbSet = GetCachedDbSet<InfiniLoreUser>();

        // Query
        IQueryable<InfiniLoreUser> query = dbSet
            .Where(ls => ls.Username == username)
            .ConditionalWhere(
                skipUserId != Guid.Empty,
                predicate: ls => ls.Id != skipUserId
            );

        // Retrieve
        bool result = await query.AnyAsync(cancellationToken: ct);
        return RepoResult.FromBool(!result);
    }
    
    public async ValueTask<RepoResult> IsExistingAuth0Id(string auth0UserId, CancellationToken ct = default) {
        // Access
        DbSet<InfiniLoreUser> dbSet = GetCachedDbSet<InfiniLoreUser>();
        
        // Query
        IQueryable<InfiniLoreUser> query = dbSet.AsNoTracking()
            .With(ByAuth0IdQuery, auth0UserId);

        // Retrieve    
        bool result = await query.AnyAsync(cancellationToken: ct);
        return RepoResult.FromBool(result);
    }


    public async ValueTask<RepoResult<InfiniLoreUser[]>> TryGetAllByAuth0IdsAsync(CancellationToken ct = default, params HashSet<string> authIds) {
        // Access
        DbSet<InfiniLoreUser> dbSet = GetCachedDbSet<InfiniLoreUser>();
        
        // Query
        IQueryable<InfiniLoreUser> query = dbSet.Where(model =>
            model.Auth0IdGoogle != null && authIds.Contains(model.Auth0IdGoogle)
            || model.Auth0Github != null && authIds.Contains(model.Auth0Github)
        );
        
        // Retrieve
        InfiniLoreUser[] result = await query.ToArrayAsync(cancellationToken: ct);
        if (result.IsEmpty()) return RepoResult<InfiniLoreUser[]>.FromFailure(RepositoryFailures.ModelsNotFound);
        return RepoResult<InfiniLoreUser[]>.FromSuccess(result);
    }
    
    #region ByAuth0Id
    private static IQueryable<InfiniLoreUser> ByAuth0IdQuery(IQueryable<InfiniLoreUser> query, string auth0Id)
        => query.Where(ls =>
            ls.Auth0IdGoogle != null && ls.Auth0IdGoogle == auth0Id
            || ls.Auth0Github != null && ls.Auth0Github == auth0Id
            || ls.Auth0MailPassword != null && ls.Auth0MailPassword == auth0Id
        );

    public async ValueTask<RepoResult<InfiniLoreUser>> TryGetByAuth0IdAsync(string auth0Id, CancellationToken ct = default) {
        // Access
        DbSet<InfiniLoreUser> dbSet = GetCachedDbSet<InfiniLoreUser>();

        // Query
        IQueryable<InfiniLoreUser> query = dbSet.With(ByAuth0IdQuery, auth0Id);
        InfiniLoreUser? result = await query
            .SingleOrDefaultAsync(cancellationToken: ct);

        // Retrieve
        if (result is null) return RepoResult<InfiniLoreUser>.FromFailure(RepositoryFailures.ModelNotFound);

        return RepoResult<InfiniLoreUser>.FromSuccess(result);
    }

    public async ValueTask<RepoResult<InfiniLoreUser>> TryGetByAuth0IdWithAutoIncludeAsync(string auth0Id, CancellationToken ct = default) {
        // Access
        DbSet<InfiniLoreUser> dbSet = GetCachedDbSet<InfiniLoreUser>();

        // Query
        IQueryable<InfiniLoreUser> query = dbSet
            .With(ByAuth0IdQuery, auth0Id)
            .With(AutoInclude);
        InfiniLoreUser? result = await query.FirstOrDefaultAsync(cancellationToken: ct);

        // Retrieve
        if (result is null) return RepoResult<InfiniLoreUser>.FromFailure(RepositoryFailures.ModelNotFound);

        return RepoResult<InfiniLoreUser>.FromSuccess(result);
    }
    #endregion
}
