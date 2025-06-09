// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace InfiniLore.Modules.Core.Server.Database.InfiniLoreUser;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IInfiniLoreUserRepository>]
public class InfiniLoreUserRepository : BasicModelRepository<InfiniLoreUserModel>, IInfiniLoreUserRepository {

    protected override IQueryable<InfiniLoreUserModel> AlwaysInclude(IQueryable<InfiniLoreUserModel> query) =>
        base.AlwaysInclude(query)
            .Include(user => user.ProfileImageMetaData);

    public async ValueTask<RepoOutcome<Guid>> TryGetIdByAuth0IdAsync(string auth0Id, CancellationToken ct = default) {
        if (auth0Id.IsNullOrWhiteSpace()) return RepoOutcome<Guid>.FromError(RepositoryFailures.ModelFailedValidation);

        DbSet<InfiniLoreUserModel> dbSet = GetCachedDbSet<InfiniLoreUserModel>();

        Guid result = await dbSet.AsNoTracking()
            .With(ByAuth0IdQuery, auth0Id)
            .Select(user => user.Id)
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);

        return result == Guid.Empty
            ? RepoOutcome<Guid>.FromError(RepositoryFailures.ModelNotFound)
            : RepoOutcome<Guid>.FromData(result);
    }

    public async ValueTask<RepoOutcome> IsUsernameTakenAsync(string username, Guid skipUserId = default, CancellationToken ct = default) {
        if (username.IsNullOrWhiteSpace()) return RepoOutcome.FromError(RepositoryFailures.ModelFailedValidation);

        DbSet<InfiniLoreUserModel> dbSet = GetCachedDbSet<InfiniLoreUserModel>();

        bool exists = await dbSet
            .Where(user => user.Username == username)
            .ConditionalWhere(skipUserId != Guid.Empty, predicate: user => user.Id != skipUserId)
            .AnyAsync(ct)
            .ConfigureAwait(false);

        return RepoOutcome.FromState(exists);
    }

    public async ValueTask<RepoOutcome> IsUsernameNotTakenAsync(string username, Guid skipUserId = default, CancellationToken ct = default) {
        if (username.IsNullOrWhiteSpace()) return RepoOutcome.FromError(RepositoryFailures.ModelFailedValidation);

        DbSet<InfiniLoreUserModel> dbSet = GetCachedDbSet<InfiniLoreUserModel>();

        bool exists = await dbSet
            .Where(user => user.Username == username)
            .ConditionalWhere(skipUserId != Guid.Empty, predicate: user => user.Id != skipUserId)
            .AnyAsync(ct)
            .ConfigureAwait(false);

        return RepoOutcome.FromState(!exists);
    }

    public async ValueTask<RepoOutcome> IsExistingAuth0Id(string auth0UserId, CancellationToken ct = default) {
        DbSet<InfiniLoreUserModel> dbSet = GetCachedDbSet<InfiniLoreUserModel>();

        bool exists = await dbSet.AsNoTracking()
            .With(ByAuth0IdQuery, auth0UserId)
            .AnyAsync(ct)
            .ConfigureAwait(false);

        return RepoOutcome.FromState(exists);
    }

    public async ValueTask<RepoOutcome<InfiniLoreUserModel[]>> TryGetAllByAuth0IdsAsync(QueryConfig config = default, CancellationToken ct = default, params HashSet<string> authIds) {
        if (authIds.Count == 0) return RepoOutcome<InfiniLoreUserModel[]>.FromError(RepositoryFailures.InvalidInput);

        DbSet<InfiniLoreUserModel> dbSet = GetCachedDbSet<InfiniLoreUserModel>();

        InfiniLoreUserModel[] result = await GetConfiguredQueryable(dbSet, config)
            .AsNoTracking()
            .Where(user =>
                user.Auth0MailPassword != null && authIds.Contains(user.Auth0MailPassword) ||
                user.Auth0IdGoogle != null && authIds.Contains(user.Auth0IdGoogle) ||
                user.Auth0Github != null && authIds.Contains(user.Auth0Github)
            )
            .ToArrayAsync(ct)
            .ConfigureAwait(false);

        return result.IsEmpty()
            ? RepoOutcome<InfiniLoreUserModel[]>.FromError(RepositoryFailures.ModelsNotFound)
            : RepoOutcome<InfiniLoreUserModel[]>.FromData(result);
    }

    public async ValueTask<RepoOutcome<InfiniLoreUserModel>> TryGetByAuth0IdAsync(string auth0Id, QueryConfig config = default, CancellationToken ct = default) {
        if (auth0Id.IsNullOrWhiteSpace())
            return RepoOutcome<InfiniLoreUserModel>.FromError(RepositoryFailures.ModelFailedValidation);

        DbSet<InfiniLoreUserModel> dbSet = GetCachedDbSet<InfiniLoreUserModel>();

        InfiniLoreUserModel? result = await GetConfiguredQueryable(dbSet, config)
            .With(ByAuth0IdQuery, auth0Id)
            .SingleOrDefaultAsync(ct)
            .ConfigureAwait(false);

        return result is null
            ? RepoOutcome<InfiniLoreUserModel>.FromError(RepositoryFailures.ModelNotFound)
            : RepoOutcome<InfiniLoreUserModel>.FromData(result);
    }

    private static IQueryable<InfiniLoreUserModel> ByAuth0IdQuery(IQueryable<InfiniLoreUserModel> query, string auth0Id) =>
        query.Where(user =>
            user.Auth0IdGoogle != null && user.Auth0IdGoogle == auth0Id ||
            user.Auth0Github != null && user.Auth0Github == auth0Id ||
            user.Auth0MailPassword != null && user.Auth0MailPassword == auth0Id
        );
}
