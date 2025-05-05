// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Server.Modules.Core.Database;
using InfiniLore.Server.Modules.Users.Database;

namespace InfiniLore.Server.Modules.LoreScopes.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ILoreScopeRepository : IOwnedModelRepository<InfiniLoreUserModel, LoreScopeModel> {
    ValueTask<Result> IsLoreScopeNameTakenAsync(string loreScopeName, Guid ownerId, CancellationToken ct = default);
    ValueTask<Result> IsLoreScopeNameNotTakenAsync(string loreScopeName, Guid ownerId, CancellationToken ct = default);
}
