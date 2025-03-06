// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Server.Contracts.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly record struct PaginationInfo(int PageNumber, int PageSize) {
    public int SkipAmount => (PageNumber - 1) * PageSize;

    public bool IsValid(out string error) {
        switch (this) {
            case { PageNumber: < 1 }: {
                error = RepositoryFailures.PaginationInvalidPageNumber;
                return false;
            }

            case { PageSize: < 1 }: {
                error = RepositoryFailures.PaginationInvalidPageSize;
                return false;
            }

            default: {
                error = RepositoryFailures.Unknown;
                return true;
            }
        }
    }

    public bool IsNotValid(out string error) => !IsValid(out error);
}
