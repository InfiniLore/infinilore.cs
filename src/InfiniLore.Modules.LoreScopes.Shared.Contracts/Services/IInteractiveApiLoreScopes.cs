// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Shared;
using InfiniLore.Modules.LoreScopes.Shared.Database;
using Microsoft.AspNetCore.Components.Forms;

namespace InfiniLore.Modules.LoreScopes.Shared.Services;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IInteractiveApiLoreScopes  {
    ValueTask<Outcome<ILoreScopeModel>> GetLoreScopeAsync(string userId, string loreScopeId, CancellationToken ct = default);
    ValueTask<PaginatedOutcome<ILoreScopeModel>> GetLoreScopesAsync(string userId, Pagination pagination, CancellationToken ct = default);
    
    ValueTask<Outcome> CreateLoreScopeAsync(string userId, string newLoreScopeName, CancellationToken ct = default);
    ValueTask<Outcome> UpsertLoreScopeImageAsync(string userId, string loreScopeId, IBrowserFile file, CancellationToken ct = default);
    ValueTask<Outcome> DeleteLoreScopesAsync(string userId, string loreScopeId, CancellationToken ct = default);
    ValueTask<Outcome> UpdateLoreScopeNameAsync(string userId, string loreScopeId, string newLoreScopeName, CancellationToken ct = default);
}
