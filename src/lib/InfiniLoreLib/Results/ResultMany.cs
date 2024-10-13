// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLoreLib.Results;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents a result that can contain multiple values and additional metadata indicating success or failure.
/// </summary>
/// <typeparam name="T">The type of the values contained in the result.</typeparam>
public class ResultMany<T>(
    bool isSuccess,
    IEnumerable<T>? values = default,
    string? errorMessage = null
) {
    /// <summary>
    /// Indicates whether the operation resulted in failure.
    /// </summary>
    /// <value>
    /// Returns <c>true</c> if the operation was not successful; otherwise, <c>false</c>.
    /// </value>
    public bool IsFailure => !IsSuccess;
    public bool IsSuccess { get; } = isSuccess;
    public IEnumerable<T>? Values { get; } = values;
    public string? ErrorMessage { get; init; } = errorMessage;

    /// <summary>
    /// Returns a successful <see cref="ResultMany{T}"/> with the provided values.
    /// </summary>
    /// <param name="value">The list of values representing the successful outcome.</param>
    /// <returns>A <see cref="ResultMany{T}"/> instance indicating success and containing the provided values.</returns>
    public static ResultMany<T> Success(IEnumerable<T> value) => new(true, value);
    /// <summary>
    /// Creates a failure result with the specified error message.
    /// </summary>
    /// <param name="errorMessage">The error message describing the failure. If no message is provided, null is used.</param>
    /// <returns>A ResultMany object representing a failure.</returns>
    public static ResultMany<T> Failure(string? errorMessage = null) => new(false, default, errorMessage);
}
