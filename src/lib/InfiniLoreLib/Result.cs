// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLoreLib;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record Result<T> (
    bool IsSuccess,
    T? Value = default,
    string? ErrorMessage = null
) {
    public bool IsFailure => !IsSuccess;
    
    public static Result<T> Success(T value) => new(true, value);
    public static Result<T> Failure(string? errorMessage = null) => new(false, default, errorMessage);
    
    public static implicit operator bool(Result<T> result) => result.IsSuccess;
}