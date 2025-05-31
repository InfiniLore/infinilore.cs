// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Shared;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly record struct PaginationInfo {

    private readonly int _pageNumber;
    public int PageNumber {
        get => _pageNumber;
        init => _pageNumber = Math.Max(1, value);
    } 
    
    private readonly int _pageSize;
    public int PageSize {
        get => _pageSize;
        init => _pageSize = value != 0 
        ? Math.Max(1, value)
        : 64;
    }
    
    public int SkipAmount => (PageNumber - 1) * PageSize;

    public PaginationInfo(int pageNumber = 1, int pageSize = 64) {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
    
    public static PaginationInfo Empty => new() {
        PageNumber = 0,
        PageSize = 0
    };
    
    public static PaginationInfo Default => new() {
        PageNumber = 1,
        PageSize = 64
    };
    
    public static PaginationInfo From(IHasPageNumber entity) => Default with {
        PageNumber = entity.PageNumber
    };

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public PaginationInfo NextPage() => this with {
        PageNumber = PageNumber + 1
    };
    
    public PaginationInfo PreviousPage() =>this with {
        PageNumber = PageNumber - 1
    };
}

public interface IHasPageNumber {
    int PageNumber { get; }
}
