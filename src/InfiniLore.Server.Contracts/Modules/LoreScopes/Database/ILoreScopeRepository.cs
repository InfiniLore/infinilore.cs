// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Server.Database.Models;

namespace InfiniLore.Server.Modules.LoreScopes.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ILoreScopeRepository : IUserDataRepository<ILoreScope> {
    ValueTask<Result> IsLoreScopeNameTakenAsync(string loreScopeName, Guid ownerId, CancellationToken ct = default);
    ValueTask<Result> IsLoreScopeNameNotTakenAsync(string loreScopeName, Guid ownerId, CancellationToken ct = default);
}
