// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Shared;

namespace InfiniLore.Modules.Core.Server.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IInfiniLoreUserRepository : IBasicModelRepository<InfiniLoreUserModel> {
    ValueTask<Outcome<InfiniLoreUserModel[]>> TryGetAllByAuth0IdsAsync(QueryConfig config = default, CancellationToken ct = default, params HashSet<string> authIds);
    ValueTask<Outcome<InfiniLoreUserModel>> TryGetByAuth0IdAsync(string auth0Id, QueryConfig config = default, CancellationToken ct = default);

    ValueTask<Outcome<Guid>> TryGetIdByAuth0IdAsync(string auth0Id, CancellationToken ct = default);

    ValueTask<Outcome> IsUsernameTakenAsync(string username, Guid skipUserId = default, CancellationToken ct = default);
    ValueTask<Outcome> IsUsernameNotTakenAsync(string username, Guid skipUserId = default, CancellationToken ct = default);

    ValueTask<Outcome> IsExistingAuth0Id(string auth0UserId, CancellationToken ct = default);
}
