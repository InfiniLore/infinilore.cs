// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Server.Database.Models.Account;

namespace InfiniLore.Server.Contracts.Database.Repositories.Account;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IUserRepository : IBasicDataRepository<InfiniLoreUser> {
    ValueTask<Result<InfiniLoreUser[]>> TryGetAllByAuth0IdsAsync(QueryConfig config = default, CancellationToken ct = default, params HashSet<string> authIds);

    ValueTask<Result<InfiniLoreUser>> TryGetByAuth0IdAsync(string auth0Id, QueryConfig config = default, CancellationToken ct = default);

    ValueTask<Result<Guid>> TryGetIdByAuth0IdAsync(string auth0Id, CancellationToken ct = default);

    ValueTask<Result> IsUsernameTakenAsync(string username, Guid skipUserId = default, CancellationToken ct = default);
    ValueTask<Result> IsUsernameNotTakenAsync(string username, Guid skipUserId = default, CancellationToken ct = default);

    ValueTask<Result> IsExistingAuth0Id(string auth0UserId, CancellationToken ct = default);
}
