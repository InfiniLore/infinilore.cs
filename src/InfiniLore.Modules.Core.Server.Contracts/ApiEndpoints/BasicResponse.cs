// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using JetBrains.Annotations;

namespace InfiniLore.Modules.Core.Server.ApiEndpoints;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract record BasicResponse {
    public required Guid Id { [UsedImplicitly] get; init; }
    public required DateTime CreatedDate { [UsedImplicitly] get; init; }
    public required DateTime LastModifiedDate { [UsedImplicitly] get; init; }
}
