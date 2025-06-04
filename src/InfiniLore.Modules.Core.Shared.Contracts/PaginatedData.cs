// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Modules.Core.Shared;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
///     Encapsulates the result of a paginated operation, providing a subset of the total data alongside pagination
///     details.
/// </summary>
/// <typeparam name="T">The type of items contained in the paginated result set.</typeparam>
/// <param name="Items">The items in the current page of the result set.</param>
/// <param name="TotalCount">The total number of items across all pages.</param>
/// <param name="CurrentPage">The current page number (starting from 1).</param>
/// <param name="TotalPages">The total number of pages in the result set.</param>
public record PaginatedData<T>(
    T[] Items,
    int TotalCount,
    int CurrentPage,
    int TotalPages
) where T : class {
    public bool HasNextPage => CurrentPage < TotalPages - 1;
    public bool HasPreviousPage => CurrentPage > 0;
    public bool IsFirstPage => CurrentPage == 0;
    public bool IsLastPage => CurrentPage == TotalPages - 1;

    public bool IsEmpty => Items.Length == 0;
    public bool IsNotEmpty => Items.Length > 0;

    public static PaginatedData<T> Empty { get; } = new(
        [],
        0,
        0,
        0
    );

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public PaginatedData<TInterface> CastTo<TInterface>() 
        where TInterface : class
        => new(
            Items.Cast<TInterface>().ToArray(),
            TotalCount,
            CurrentPage,
            TotalPages
        );

    public bool TryGetNextPage(int pageSize, out Pagination pagination) {
        if (!HasNextPage) {
            pagination = Pagination.Default with {
                PageSize = pageSize,
            };
            return false;
        }
        
        pagination = new Pagination(CurrentPage + 1, pageSize);
        return true;
    }

    public bool TryGetPreviousPage(int pageSize, out Pagination pagination) {
        if (!HasPreviousPage) {
            pagination = Pagination.Empty;
            return false;
        }
        
        pagination = new Pagination(CurrentPage - 1, pageSize);
        return true;
    }
    
    public Pagination GetCurrentPage(int pageSize) => new(CurrentPage, pageSize);
}
