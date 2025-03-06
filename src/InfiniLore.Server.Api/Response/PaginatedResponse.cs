// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using JetBrains.Annotations;

namespace InfiniLore.Server.Api.Response;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract record PaginatedResponse<T> {
    public required T[] Items { [UsedImplicitly] get; init; }
    public int TotalCount { [UsedImplicitly] get; init; }
    public int CurrentPage { [UsedImplicitly] get; init; }
    public int TotalPages { [UsedImplicitly] get; init; }
}
