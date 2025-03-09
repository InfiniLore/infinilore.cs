// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using JetBrains.Annotations;

namespace InfiniLore.Server.Api.Responses;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract record BasicDataResponse {
    public required Guid Id { [UsedImplicitly] get; init; }
    public required DateTime CreatedDate { [UsedImplicitly] get; init; }
    public required DateTime LastModifiedDate { [UsedImplicitly] get; init; }
}
