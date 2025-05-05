// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Shared.Modules.LoreScopes.Database;

namespace InfiniLore.Shared.Modules.LoreScopes.Services;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ILoreScopeInteractiveApi {
    ValueTask<Result<PaginatedData<ILoreScopeModel>>> GetLoreScopesAsync(string userId, CancellationToken ct = default);
}
