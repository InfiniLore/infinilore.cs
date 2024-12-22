// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;

namespace InfiniLore.Server.Types;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly record struct PaginationInfo(int PageNumber, int PageSize) {
    public int SkipAmount => (PageNumber - 1) * PageSize;
    internal const string PageNumberErrorMessage = "Page number must be greater than 0.";
    internal const string PageSizeErrorMessage = "Page size must be greater than 0.";

    public bool IsValid(out Failure<string> error) {
        if (PageNumber < 1) {
            error = new Failure<string>(PageNumberErrorMessage);
            return false;
        }

        if (PageSize < 1) {
            error = new Failure<string>(PageSizeErrorMessage);
            return false;
        }

        error = new Failure<string>();
        return true;
    }

    public bool IsNotValid(out Failure<string> error) => !IsValid(out error);
}
