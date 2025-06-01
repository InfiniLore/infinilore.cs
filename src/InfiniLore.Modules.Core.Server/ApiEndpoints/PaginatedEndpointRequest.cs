// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Shared;
using JetBrains.Annotations;

namespace InfiniLore.Modules.Core.Server.ApiEndpoints;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record PaginatedEndpointRequest : IHasReverse, IHasPageNumber {
    [BindFrom("pageNumber")] public int PageNumber { get; [UsedImplicitly] set; } = 1;
    [BindFrom("reverse")] public bool Reverse { get; [UsedImplicitly] set; } = false;
}
