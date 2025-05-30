// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Shared.Modules.LoreScopes.Database;

namespace InfiniLore.Shared.Modules.LoreScopes.Services;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ILoreScopeInteractiveApi  {
    ValueTask<Result<ILoreScopeModel>> GetLoreScopeAsync(string userId, string loreScopeId, CancellationToken ct = default);
    ValueTask<PaginatedResult<ILoreScopeModel>> GetLoreScopesAsync(string userId, CancellationToken ct = default);
    
    ValueTask<Result> CreateLoreScopeAsync(string userId, string newLoreScopeName, CancellationToken ct = default);
    ValueTask<Result> UpsertLoreScopeImageAsync(string userId, string loreScopeId, string fileName, string contentType, Stream file, CancellationToken ct = default);
    ValueTask<Result> DeleteLoreScopesAsync(string loreScopeId, CancellationToken ct = default);
}
