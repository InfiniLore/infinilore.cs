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
        init => _pageNumber = Math.Max(0, value);
    } 
    
    private readonly int _pageSize;
    public int PageSize {
        get => _pageSize;
        init => _pageSize = Math.Max(1, value);
    }
    
    public int SkipAmount => PageNumber * PageSize;

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public PaginationInfo(int pageNumber = 0, int pageSize = 64) {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
    
    public static PaginationInfo Empty => new() {
        PageNumber = 0,
        PageSize = 0
    };
    
    public static PaginationInfo Default => new() {
        PageNumber = 0,
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
