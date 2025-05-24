// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Server.Modules.Core.Database;

namespace InfiniLore.Server.Modules.LoreScopes.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ILoreScopeRepository : IOwnedModelRepository<InfiniLoreUserModel, LoreScopeModel> {
    ValueTask<Result> IsLoreScopeNameTakenAsync(string loreScopeName, Guid ownerId, Guid notIncludedId = default, CancellationToken ct = default);
    ValueTask<Result> IsLoreScopeNameNotTakenAsync(string loreScopeName, Guid ownerId, Guid notIncludedId = default, CancellationToken ct = default);
}
