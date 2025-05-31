// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Shared;
using JetBrains.Annotations;

namespace InfiniLore.Modules.Core.Server.ApiEndpoints;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record PaginatedRequest : IHasReverse, IHasPageNumber {
    public int PageNumber { get; [UsedImplicitly] init; } = 1;
    public bool Reverse { get; [UsedImplicitly] init; } = false;
}
