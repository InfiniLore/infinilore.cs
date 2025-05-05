// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Shared;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly record struct PaginationInfo(int PageNumber, int PageSize) {
    public int SkipAmount => (PageNumber - 1) * PageSize;
    
    public static PaginationInfo Empty => new(0, 0);
    public static PaginationInfo Default => new(1, 64);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private bool IsValid() {
        return this switch {
            { PageNumber: < 1 } => false,
            { PageSize: < 1 } => false,
            _ => true
        };
    }

    public bool IsNotValid() => !IsValid();
    
    public PaginationInfo NextPage() => this with {
        PageNumber = PageNumber + 1
    };
    
    public PaginationInfo PreviousPage() => this with {
        PageNumber = Math.Min(PageNumber - 1, 1)
    };
}
