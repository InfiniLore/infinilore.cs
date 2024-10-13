// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

namespace InfiniLoreLib.Results;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents the outcome of an operation, encapsulating a success flag, a value, and an optional error message.
/// </summary>
/// <typeparam name="T">The type of the value provided on success.</typeparam>
/// <param name="isSuccess">Indicates whether the operation was successful.</param>
/// <param name="value">The value resulting from a successful operation.</param>
/// <param name="errorMessage">The error message describing the failure, if any.</param>
public class Result<T>(
    bool isSuccess,
    T? value = default,
    string? errorMessage = null
) {
    /// <summary>
    /// Gets a value indicating whether the result represents a failure.
    /// </summary>
    public virtual bool IsFailure => !IsSuccess;

    /// <summary>
    /// Indicates whether the operation was successful.
    /// </summary>
    public virtual bool IsSuccess { get; } = isSuccess;

    /// <summary>
    /// The value resulting from a successful operation.
    /// </summary>
    [MemberNotNullWhen(true, nameof(IsSuccess))]
    [MemberNotNullWhen(false, nameof(IsFailure))]
    public T? Value { get; } = value;

    /// <summary>
    /// The error message describing the failure, if any.
    /// </summary>
    public string? ErrorMessage { get; } = errorMessage;

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

}
