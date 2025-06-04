// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Modules.Core.Server.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IInfiniLoreUserRepository : IBasicModelRepository<InfiniLoreUserModel> {
    ValueTask<RepoOutcome<InfiniLoreUserModel[]>> TryGetAllByAuth0IdsAsync(QueryConfig config = default, CancellationToken ct = default, params HashSet<string> authIds);
    ValueTask<RepoOutcome<InfiniLoreUserModel>> TryGetByAuth0IdAsync(string auth0Id, QueryConfig config = default, CancellationToken ct = default);

    ValueTask<RepoOutcome<Guid>> TryGetIdByAuth0IdAsync(string auth0Id, CancellationToken ct = default);

    ValueTask<RepoOutcome> IsUsernameTakenAsync(string username, Guid skipUserId = default, CancellationToken ct = default);
    ValueTask<RepoOutcome> IsUsernameNotTakenAsync(string username, Guid skipUserId = default, CancellationToken ct = default);

    ValueTask<RepoOutcome> IsExistingAuth0Id(string auth0UserId, CancellationToken ct = default);
}
