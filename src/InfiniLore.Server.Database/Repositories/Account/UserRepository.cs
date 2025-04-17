// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories;
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

    public async ValueTask<Result<Guid>> TryGetIdByAuth0IdAsync(string auth0Id, CancellationToken ct = default) {
        if (auth0Id.IsNullOrWhiteSpace()) return Result<Guid>.FromError(RepositoryFailures.ModelFailedValidation);

        // Access
        DbSet<InfiniLoreUser> dbSet = GetCachedDbSet<InfiniLoreUser>();

        // Query
        IQueryable<Guid> query = dbSet.AsNoTracking()
            .With(ByAuth0IdQuery, auth0Id)
            .Select(ls => ls.Id);

        // Retrieve
        Guid result = await query.FirstOrDefaultAsync(cancellationToken: ct);
        if (result == Guid.Empty) return Result<Guid>.FromError(RepositoryFailures.ModelNotFound);

        return Result<Guid>.FromSuccess(result);
    }

    public async ValueTask<Result> IsUsernameTakenAsync(string username, Guid skipUserId = default, CancellationToken ct = default) {
        if (username.IsNullOrWhiteSpace()) return Result.FromError(RepositoryFailures.ModelFailedValidation);

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
        return Result.FromState(result);
    }

    public async ValueTask<Result> IsUsernameNotTakenAsync(string username, Guid skipUserId = default, CancellationToken ct = default) {
        if (username.IsNullOrWhiteSpace()) return Result.FromError(RepositoryFailures.ModelFailedValidation);

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
        return Result.FromState(!result);
    }

    public async ValueTask<Result> IsExistingAuth0Id(string auth0UserId, CancellationToken ct = default) {
        // Access
        DbSet<InfiniLoreUser> dbSet = GetCachedDbSet<InfiniLoreUser>();

        // Query
        IQueryable<InfiniLoreUser> query = dbSet.AsNoTracking()
            .With(ByAuth0IdQuery, auth0UserId);

        // Retrieve    
        bool result = await query.AnyAsync(cancellationToken: ct);
        return Result.FromState(result);
    }


    public async ValueTask<Result<InfiniLoreUser[]>> TryGetAllByAuth0IdsAsync(QueryConfig config = default, CancellationToken ct = default, params HashSet<string> authIds) {
        // Access
        DbSet<InfiniLoreUser> dbSet = GetCachedDbSet<InfiniLoreUser>();

        // Query
        IQueryable<InfiniLoreUser> query = dbSet
            .AsNoTracking()
            .ConditionalWith(config.AutoInclude, AutoInclude)
            .ConditionalReverse(config.Reverse)
            .Where(model =>
                model.Auth0MailPassword != null && authIds.Contains(model.Auth0MailPassword)
                || model.Auth0IdGoogle != null && authIds.Contains(model.Auth0IdGoogle)
                || model.Auth0Github != null && authIds.Contains(model.Auth0Github)
            );

        // Retrieve
        InfiniLoreUser[] result = await query.ToArrayAsync(cancellationToken: ct);
        if (result.IsEmpty()) return Result<InfiniLoreUser[]>.FromError(RepositoryFailures.ModelsNotFound);

        return Result<InfiniLoreUser[]>.FromSuccess(result);
    }

    #region ByAuth0Id
    private static IQueryable<InfiniLoreUser> ByAuth0IdQuery(IQueryable<InfiniLoreUser> query, string auth0Id)
        => query.Where(ls =>
            ls.Auth0IdGoogle != null && ls.Auth0IdGoogle == auth0Id
            || ls.Auth0Github != null && ls.Auth0Github == auth0Id
            || ls.Auth0MailPassword != null && ls.Auth0MailPassword == auth0Id
        );

    public async ValueTask<Result<InfiniLoreUser>> TryGetByAuth0IdAsync(string auth0Id, QueryConfig config = default, CancellationToken ct = default) {
        // Access
        DbSet<InfiniLoreUser> dbSet = GetCachedDbSet<InfiniLoreUser>();

        // Query
        IQueryable<InfiniLoreUser> query = dbSet
            .ConditionalWith(config.AutoInclude, AutoInclude)
            .With(ByAuth0IdQuery, auth0Id);

        InfiniLoreUser? result = await query
            .SingleOrDefaultAsync(cancellationToken: ct);

        // Retrieve
        if (result is null) return Result<InfiniLoreUser>.FromError(RepositoryFailures.ModelNotFound);

        return Result<InfiniLoreUser>.FromSuccess(result);
    }

    public async ValueTask<Result<InfiniLoreUser>> TryGetByAuth0IdWithAutoIncludeAsync(string auth0Id, QueryConfig config = default, CancellationToken ct = default) {
        // Access
        DbSet<InfiniLoreUser> dbSet = GetCachedDbSet<InfiniLoreUser>();

        // Query
        IQueryable<InfiniLoreUser> query = dbSet
            .ConditionalWith(config.AutoInclude, AutoInclude)
            .ConditionalReverse(config.Reverse)
            .With(ByAuth0IdQuery, auth0Id);

        InfiniLoreUser? result = await query.FirstOrDefaultAsync(cancellationToken: ct);

        // Retrieve
        if (result is null) return Result<InfiniLoreUser>.FromError(RepositoryFailures.ModelNotFound);

        return Result<InfiniLoreUser>.FromSuccess(result);
    }
    #endregion
}
