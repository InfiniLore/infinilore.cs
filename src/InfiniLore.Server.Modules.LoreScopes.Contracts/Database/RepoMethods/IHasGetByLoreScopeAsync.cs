// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Server.Modules.Core.Database;
using InfiniLore.Shared;

namespace InfiniLore.Server.Modules.LoreScopes.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasGetByLoreScopeAsync<TInterface> where TInterface : OwnedModel<LoreScopeModel> {
    ValueTask<Result<TInterface[]>> GetByLoreScopeAsync(Guid loreScopeId, QueryConfig config = default, CancellationToken ct = default);
    ValueTask<Core.Database.PaginatedResult<TInterface>> GetByLoreScopeAsync(Guid loreScopeId, PaginationInfo pageInfo, QueryConfig config = default, CancellationToken ct = default);
}
