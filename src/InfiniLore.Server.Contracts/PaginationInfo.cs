// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Database;

namespace InfiniLore.Server;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly record struct PaginationInfo(int PageNumber = 1, int PageSize = 64) {
    public int SkipAmount => (PageNumber - 1) * PageSize;
    public static PaginationInfo Empty => new(0);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
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
