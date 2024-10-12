// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLoreLib.Results;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record BoolResult(
    bool IsSuccess,
    string? ErrorMessage = null
) : Result<bool>(
    IsSuccess,
    IsSuccess,
    ErrorMessage
) {
    public static BoolResult Success() => new(true);
    public new static BoolResult Failure(string? errorMessage = null) => new(false, ErrorMessage: errorMessage);
}
