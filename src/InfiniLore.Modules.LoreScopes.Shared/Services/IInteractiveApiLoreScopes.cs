// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Modules.LoreScopes.Shared.Database;
using InfiniLore.Shared;

namespace InfiniLore.Modules.LoreScopes.Shared.Services;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IInteractiveApiLoreScopes  {
    ValueTask<Result<ILoreScopeModel>> GetLoreScopeAsync(string userId, string loreScopeId, CancellationToken ct = default);
    ValueTask<PaginatedResult<ILoreScopeModel>> GetLoreScopesAsync(string userId, CancellationToken ct = default);
    
    ValueTask<Result> CreateLoreScopeAsync(string userId, string newLoreScopeName, CancellationToken ct = default);
    ValueTask<Result> UpsertLoreScopeImageAsync(string userId, string loreScopeId, string fileName, string contentType, Stream file, CancellationToken ct = default);
    ValueTask<Result> DeleteLoreScopesAsync(string loreScopeId, CancellationToken ct = default);
}
