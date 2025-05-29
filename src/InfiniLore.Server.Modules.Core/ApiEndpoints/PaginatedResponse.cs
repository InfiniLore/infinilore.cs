// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using JetBrains.Annotations;

namespace InfiniLore.Server.Modules.Core.ApiEndpoints;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract record PaginatedResponse<T> where T : BasicResponse {
    public required T[] Items { [UsedImplicitly] get; set; }
    public int TotalCount { [UsedImplicitly] get; init; }
    public int CurrentPage { [UsedImplicitly] get; init; }
    public int TotalPages { [UsedImplicitly] get; init; }
}
