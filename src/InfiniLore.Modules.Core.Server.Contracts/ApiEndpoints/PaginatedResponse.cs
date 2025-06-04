// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using JetBrains.Annotations;

namespace InfiniLore.Modules.Core.Server.ApiEndpoints;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract record PaginatedResponse<T> {
    public T[] Items { [UsedImplicitly] get; set; } = Array.Empty<T>();
    public int TotalCount { [UsedImplicitly] get; init; }
    public int CurrentPage { [UsedImplicitly] get; init; }
    public int TotalPages { [UsedImplicitly] get; init; }
}
