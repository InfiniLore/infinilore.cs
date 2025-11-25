// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Core.Pagination;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly record struct PaginationData(int PageNumber = 0, int PageSize = PaginationData.DefaultPageSize) {
    public int PageNumber {
        get;
        init => field = Math.Max(0, value);
    } = PageNumber;

    public int PageSize {
        get;
        init => field = Math.Max(1, value);
    } = PageSize;

    public int SkipAmount => PageNumber * PageSize;
    
    public const int DefaultPageSize = 64;

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    
    public static PaginationData Empty => new() {
        PageNumber = 0,
        PageSize = 0
    };
    
    public static PaginationData Default => new() {
        PageNumber = 0,
        PageSize = 64
    };
    
    public static PaginationData From(IHasPageNumber entity) => Default with {
        PageNumber = entity.PageNumber
    };
    
    public static PaginationData From(IHasPageSize entity) => Default with {
        PageSize = entity.PageSize
    };

    public static PaginationData From<T>(T entity) where T : IHasPageNumber, IHasPageSize => Default with {
        PageNumber = entity.PageNumber,
        PageSize = entity.PageSize
    };

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public PaginationData NextPage() => this with {
        PageNumber = PageNumber + 1
    };
    
    public PaginationData PreviousPage() =>this with {
        PageNumber = PageNumber - 1
    };
}

public interface IHasPageNumber {
    int PageNumber { get; }
}

public interface IHasPageSize {
    int PageSize { get; }   
}
