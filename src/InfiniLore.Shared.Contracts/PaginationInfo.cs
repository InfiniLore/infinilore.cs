// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Shared;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly struct PaginationInfo(int pageNumber, int pageSize) : IEquatable<PaginationInfo> {
    public readonly int PageNumber = Math.Max(1, pageNumber);
    public readonly int PageSize = pageSize != 0 
        ? Math.Max(1, pageSize)
        : 64;
    
    public int SkipAmount => (pageNumber - 1) * pageSize;
    
    public static PaginationInfo Empty => new(0, 0);
    public static PaginationInfo Default => new(1, 64);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public PaginationInfo NextPage() => new(PageNumber + 1, pageSize);
    public PaginationInfo PreviousPage() => new(PageNumber - 1, pageSize);
    
    public static bool operator ==(PaginationInfo left, PaginationInfo right) => left.Equals(right);
    public static bool operator !=(PaginationInfo left, PaginationInfo right) => !(left == right);
    public bool Equals(PaginationInfo other) => PageNumber == other.PageNumber && PageSize == other.PageSize;
    public override bool Equals(object? obj) => obj is PaginationInfo other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(PageNumber, PageSize);
}
