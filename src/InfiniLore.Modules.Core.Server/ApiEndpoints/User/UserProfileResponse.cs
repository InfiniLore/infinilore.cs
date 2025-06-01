// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Shared.Database;
using JetBrains.Annotations;

namespace InfiniLore.Modules.Core.Server.ApiEndpoints.User;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record UserProfileResponse : IInfiniLoreUserModel {
    public required Guid Id { [UsedImplicitly] get; init; }
    public required DateTime CreatedDate { [UsedImplicitly] get; init; }
    public required string Username { [UsedImplicitly] get; init; }
    public required string? ProfileImageUrl { [UsedImplicitly] get; init; }
}
