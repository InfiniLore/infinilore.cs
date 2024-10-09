// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLoreLib;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Result<T> {
    public T? Value { get; set; }
    public bool IsSuccess { get; set; }
    public bool IsFailure {get => !IsSuccess; set => IsSuccess = !value; }
    public string? ErrorMessage { get; set; }

    public static Result<T> Success(T value) => new() { Value = value, IsSuccess = true };
    public static Result<T> Failure(string? errorMessage = null) => new() { ErrorMessage = errorMessage, IsFailure = true };
    
    public static implicit operator bool(Result<T> result) => result.IsSuccess;
}
