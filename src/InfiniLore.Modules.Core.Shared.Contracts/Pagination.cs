// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Modules.Core.Shared;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly record struct Pagination {
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
    public Pagination(int pageNumber = 0, int pageSize = 64) {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
    
    public static Pagination Empty => new() {
        PageNumber = 0,
        PageSize = 0
    };
    
    public static Pagination Default => new() {
        PageNumber = 0,
        PageSize = 64
    };
    
    public static Pagination From(IHasPageNumber entity) => Default with {
        PageNumber = entity.PageNumber
    };
    
    public static Pagination From(IHasPageSize entity) => Default with {
        PageSize = entity.PageSize
    };

    public static Pagination From<T>(T entity) where T : IHasPageNumber, IHasPageSize => Default with {
        PageNumber = entity.PageNumber,
        PageSize = entity.PageSize
    };

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public Pagination NextPage() => this with {
        PageNumber = PageNumber + 1
    };
    
    public Pagination PreviousPage() =>this with {
        PageNumber = PageNumber - 1
    };
}

public interface IHasPageNumber {
    int PageNumber { get; }
}

public interface IHasPageSize {
    int PageSize { get; }   
}
