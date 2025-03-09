// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using JetBrains.Annotations;

namespace InfiniLore.Server.Api.Responses.Data.User.LoreScopes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record LoreScopeResponse : UserDataResponse {
    public required string Name { [UsedImplicitly] get; init; }
    public string? Description { [UsedImplicitly] get; init; }
}
