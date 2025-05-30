// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Server.ApiEndpoints;
using JetBrains.Annotations;

namespace InfiniLore.Modules.LoreScopes.Server.ApiEndpoints;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record LoreScopeResponse : OwnedResponse {
    public required string Name { [UsedImplicitly] get; init; }
    public required string? Description { [UsedImplicitly] get; init; }
    
    public string? ImageUrl { [UsedImplicitly] get; set; }
}
