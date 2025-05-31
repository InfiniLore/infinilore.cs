// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Server;
using Microsoft.EntityFrameworkCore;

namespace InfiniLore.Modules.Core.Server.Database.InfiniLoreUser;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IInfiniLoreUserRepository>]
public class InfiniLoreUserRepository : BasicModelRepository<InfiniLoreUserModel>, IInfiniLoreUserRepository {

    protected override IQueryable<InfiniLoreUserModel> AlwaysInclude(IQueryable<InfiniLoreUserModel> query) 
        => base.AlwaysInclude(query)
            .Include(user => user.ProfileImageMetaData)    
    ;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async ValueTask<Result<Guid>> TryGetIdByAuth0IdAsync(string auth0Id, CancellationToken ct = default) {
        if (auth0Id.IsNullOrWhiteSpace()) return Result<Guid>.FromError(RepositoryFailures.ModelFailedValidation);

        // Access
        DbSet<InfiniLoreUserModel> dbSet = GetCachedDbSet<InfiniLoreUserModel>();

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
        DbSet<InfiniLoreUserModel> dbSet = GetCachedDbSet<InfiniLoreUserModel>();

        // Query
        IQueryable<InfiniLoreUserModel> query = dbSet
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
        DbSet<InfiniLoreUserModel> dbSet = GetCachedDbSet<InfiniLoreUserModel>();

        // Query
        IQueryable<InfiniLoreUserModel> query = dbSet
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
        DbSet<InfiniLoreUserModel> dbSet = GetCachedDbSet<InfiniLoreUserModel>();

        // Query
        IQueryable<InfiniLoreUserModel> query = dbSet.AsNoTracking()
            .With(ByAuth0IdQuery, auth0UserId);

        // Retrieve    
        bool result = await query.AnyAsync(cancellationToken: ct);
        return Result.FromState(result);
    }


    public async ValueTask<Result<InfiniLoreUserModel[]>> TryGetAllByAuth0IdsAsync(QueryConfig config = default, CancellationToken ct = default, params HashSet<string> authIds) {
        // Access
        DbSet<InfiniLoreUserModel> dbSet = GetCachedDbSet<InfiniLoreUserModel>();

        // Query
        IQueryable<InfiniLoreUserModel> query = GetConfiguredQueryable(dbSet, config)
            .AsNoTracking()
            .Where(model =>
                model.Auth0MailPassword != null && authIds.Contains(model.Auth0MailPassword)
                || model.Auth0IdGoogle != null && authIds.Contains(model.Auth0IdGoogle)
                || model.Auth0Github != null && authIds.Contains(model.Auth0Github)
            );

        // Retrieve
        InfiniLoreUserModel[] result = await query.ToArrayAsync(cancellationToken: ct);
        if (result.IsEmpty()) return Result<InfiniLoreUserModel[]>.FromError(RepositoryFailures.ModelsNotFound);

        return Result<InfiniLoreUserModel[]>.FromSuccess(result);
    }

    #region ByAuth0Id
    private static IQueryable<InfiniLoreUserModel> ByAuth0IdQuery(IQueryable<InfiniLoreUserModel> query, string auth0Id)
        => query.Where(ls =>
            ls.Auth0IdGoogle != null && ls.Auth0IdGoogle == auth0Id
            || ls.Auth0Github != null && ls.Auth0Github == auth0Id
            || ls.Auth0MailPassword != null && ls.Auth0MailPassword == auth0Id
        );

    public async ValueTask<Result<InfiniLoreUserModel>> TryGetByAuth0IdAsync(string auth0Id, QueryConfig config = default, CancellationToken ct = default) {
        // Access
        DbSet<InfiniLoreUserModel> dbSet = GetCachedDbSet<InfiniLoreUserModel>();

        // Query
        IQueryable<InfiniLoreUserModel> query = GetConfiguredQueryable(dbSet, config)
            .With(ByAuth0IdQuery, auth0Id);

        InfiniLoreUserModel? result = await query
            .SingleOrDefaultAsync(cancellationToken: ct);

        // Retrieve
        if (result is null) return Result<InfiniLoreUserModel>.FromError(RepositoryFailures.ModelNotFound);

        return Result<InfiniLoreUserModel>.FromSuccess(result);
    }
    #endregion
}
