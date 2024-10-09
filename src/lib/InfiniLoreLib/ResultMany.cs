// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLoreLib;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record ResultMany<T> (
    bool IsSuccess,
    IEnumerable<T>? Values = default,
    string? ErrorMessage = null
) {
    public bool IsFailure => !IsSuccess;
    
    public static ResultMany<T> Success(IEnumerable<T> value) => new(true, value);
    public static ResultMany<T> Failure(string? errorMessage = null) => new(false, default, errorMessage);
    
    public static implicit operator bool(ResultMany<T> result) => result.IsSuccess;
}
