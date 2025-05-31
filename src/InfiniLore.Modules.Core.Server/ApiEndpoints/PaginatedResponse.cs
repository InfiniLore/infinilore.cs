// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using JetBrains.Annotations;

namespace InfiniLore.Modules.Core.Server.ApiEndpoints;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record PaginatedResponse<T> where T : BasicResponse {
    public T[] Items { [UsedImplicitly] get; set; } = Array.Empty<T>();
    public int TotalCount { [UsedImplicitly] get; init; }
    public int CurrentPage { [UsedImplicitly] get; init; }
    public int TotalPages { [UsedImplicitly] get; init; }
}
