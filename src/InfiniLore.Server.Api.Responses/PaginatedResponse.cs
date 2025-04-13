// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using JetBrains.Annotations;

namespace InfiniLore.Server.Api.Responses;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// A response which allows pagination over its elements of type <typeparamref name="T"/>.
/// </summary>
public abstract record PaginatedResponse<T> {
    public required T[] Items { [UsedImplicitly] get; init; }
    public int TotalCount { [UsedImplicitly] get; init; }
    public int CurrentPage { [UsedImplicitly] get; init; }
    public int TotalPages { [UsedImplicitly] get; init; }
}
