// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Modules.Core.ApiEndpoints;
using JetBrains.Annotations;

namespace InfiniLore.Server.Modules.LoreScopes.ApiEndpoints;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record LoreScopeResponse : OwnedResponse {
    public required string Name { [UsedImplicitly] get; init; }
    public required string? Description { [UsedImplicitly] get; init; }
}
