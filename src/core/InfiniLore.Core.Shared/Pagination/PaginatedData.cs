// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Core.Pagination;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
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
        Array.Empty<T>(),
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

    public bool TryGetNextPage(int pageSize, out PaginationData pagination) {
        if (!HasNextPage) {
            pagination = PaginationData.Default with {
                PageSize = pageSize,
            };
            return false;
        }

        pagination = new PaginationData(CurrentPage + 1, pageSize);
        return true;
    }

    public bool TryGetPreviousPage(int pageSize, out PaginationData pagination) {
        if (!HasPreviousPage) {
            pagination = PaginationData.Empty;
            return false;
        }

        pagination = new PaginationData(CurrentPage - 1, pageSize);
        return true;
    }

    public PaginationData GetCurrentPage(int pageSize) => new(CurrentPage, pageSize);
}
