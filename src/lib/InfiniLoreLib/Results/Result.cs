// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLoreLib.Results;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents the outcome of an operation, encapsulating a success flag, a value, and an optional error message.
/// </summary>
/// <typeparam name="T">The type of the value provided on success.</typeparam>
/// <param name="IsSuccess">Indicates whether the operation was successful.</param>
/// <param name="Value">The value resulting from a successful operation.</param>
/// <param name="ErrorMessage">The error message describing the failure, if any.</param>
public record Result<T>(
    bool IsSuccess,
    T? Value = default,
    string? ErrorMessage = null
) {
    /// <summary>
    /// Gets a value indicating whether the result represents a failure.
    /// </summary>
    /// <value>
    /// True if the result represents a failure; otherwise, false.
    /// </value>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Creates a successful Result object with the specified value.
    /// </summary>
    /// <param name="value">The value to be encapsulated in the successful Result.</param>
    /// <returns>A Result object indicating a successful operation.</returns>
    public static Result<T> Success(T value) => new(true, value);
    /// <summary>
    /// Creates a failure result with an optional error message.
    /// </summary>
    /// <param name="errorMessage">An optional error message describing the failure.</param>
    /// <returns>A <see cref="Result{T}"/> instance representing the failure.</returns>
    public static Result<T> Failure(string? errorMessage = null) => new(false, default, errorMessage);
    
    /// <summary>
    /// Implicitly converts a <see cref="Result{T}"/> to a boolean value indicating success.
    /// </summary>
    /// <param name="result">The <see cref="Result{T}"/> instance to be converted.</param>
    /// <returns><c>true</c> if the result indicates success; otherwise, <c>false</c>.</returns>
    public static implicit operator bool(Result<T> result) => result.IsSuccess;
}
