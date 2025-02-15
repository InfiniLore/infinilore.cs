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
    private static readonly Failure<string> PageNumberError = new(PageNumberErrorMessage);
    internal const string PageSizeErrorMessage = "Page size must be greater than 0.";
    private static readonly Failure<string> PageSizeError = new(PageSizeErrorMessage);

    public bool IsValid(out Failure<string> error) {
        switch (this) {
            case { PageNumber: < 1 }: {
                error = PageNumberError;
                return false;
            }

            case { PageSize: < 1 }: {
                error = PageSizeError;
                return false;
            }

            default: {
                error = Failure<string>.Empty;
                return true;
            }
        }
    }

    public bool IsNotValid(out Failure<string> error) => !IsValid(out error);
}
