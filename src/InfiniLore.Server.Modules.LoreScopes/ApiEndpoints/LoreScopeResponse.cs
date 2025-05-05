// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Modules.Core.ApiEndpoints;
using JetBrains.Annotations;

namespace InfiniLore.Server.Modules.LoreScopes.ApiEndpoints;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record LoreScopeResponse : UserDataResponse {
    public required string Name { [UsedImplicitly] get; init; }
    public string? Description { [UsedImplicitly] get; init; }
}
