// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Modules.Core.ApiEndpoints;
using JetBrains.Annotations;

namespace InfiniLore.Server.Modules.Users.ApiEndpoints;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record UserProfileResponse : BasicResponse {
    public required string Username { [UsedImplicitly] get; init; }
}
