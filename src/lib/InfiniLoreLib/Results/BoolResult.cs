// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLoreLib.Results;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents the result of a boolean operation, encapsulating a success flag and an optional error message, and deriving from the generic <see cref="Result{T}"/> class with a boolean value.
/// </summary>
/// <param name="isSuccess">Indicates whether the operation was successful.</param>
/// <param name="errorMessage">The error message describing the failure, if any.</param>
public class BoolResult(bool isSuccess, string? errorMessage = null) : Result<bool>(isSuccess, isSuccess, errorMessage) {
    #region Fix for MemberNotNullWhen
    /// Indicates whether the operation resulted in a failure.
    public override bool IsFailure => !IsSuccess;
    /// <inheritdoc cref="Result{T}.IsSuccess"/>
    public override bool IsSuccess { get; } = isSuccess;
    #endregion

    /// <summary>
    /// Represents a successful result.
    /// </summary>
    /// <returns>
    /// A <see cref="BoolResult"/> indicating a successful operation with no error message.
    /// </returns>
    public static BoolResult Success() => new(true);

    /// <summary>
    /// Creates a BoolResult indicating failure with an optional error message.
    /// </summary>
    /// <param name="errorMessage">An optional error message providing details about the failure.</param>
    /// <returns>A BoolResult indicating failure.</returns>
    public new static BoolResult Failure(string? errorMessage = null) => new(false, errorMessage);
}
