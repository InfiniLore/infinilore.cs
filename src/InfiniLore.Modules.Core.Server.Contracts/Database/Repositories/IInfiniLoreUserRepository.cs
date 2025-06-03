// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Modules.Core.Server.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IInfiniLoreUserRepository : IBasicModelRepository<InfiniLoreUserModel> {
    ValueTask<Shared.Outcome<InfiniLoreUserModel[]>> TryGetAllByAuth0IdsAsync(QueryConfig config = default, CancellationToken ct = default, params HashSet<string> authIds);
    ValueTask<Shared.Outcome<InfiniLoreUserModel>> TryGetByAuth0IdAsync(string auth0Id, QueryConfig config = default, CancellationToken ct = default);

    ValueTask<Shared.Outcome<Guid>> TryGetIdByAuth0IdAsync(string auth0Id, CancellationToken ct = default);

    ValueTask<Shared.Outcome> IsUsernameTakenAsync(string username, Guid skipUserId = default, CancellationToken ct = default);
    ValueTask<Shared.Outcome> IsUsernameNotTakenAsync(string username, Guid skipUserId = default, CancellationToken ct = default);

    ValueTask<Shared.Outcome> IsExistingAuth0Id(string auth0UserId, CancellationToken ct = default);
}
