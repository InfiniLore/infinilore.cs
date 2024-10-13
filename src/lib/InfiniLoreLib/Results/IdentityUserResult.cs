// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;

namespace InfiniLoreLib.Results;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents the result of an operation that returns an <see cref="IdentityUser"/> object, encapsulating a success flag, the user, and an optional error message.
/// </summary>
/// <typeparam name="T">The type of the user object, which must inherit from <see cref="IdentityUser"/>.</typeparam>
/// <param name="isSuccess">Indicates whether the operation was successful.</param>
/// <param name="value">The user object returned by the operation, if successful.</param>
/// <param name="errorMessage">The error message describing the failure, if any.</param>
public class IdentityUserResult<T>(bool isSuccess, T? value = default, string? errorMessage = null) : Result<T>(isSuccess, value, errorMessage) where T : IdentityUser {
    #region Fix for MemberNotNullWhen
    /// Indicates whether the operation resulted in a failure. It returns the negation of the `IsSuccess` property.
    public override bool IsFailure => !IsSuccess;
    /// <summary>
    /// Indicates whether the operation was successful.
    /// </summary>
    /// <inheritdoc cref="Result{T}.IsSuccess"/>
    public override bool IsSuccess { get; } = isSuccess;
    #endregion

    /// <summary>
    /// Gets the user associated with this result.
    /// </summary>
    /// <remarks>
    /// This property will be populated only if the operation resulting in this object was successful.
    /// </remarks>
    [MemberNotNullWhen(true, nameof(IsSuccess))]
    [MemberNotNullWhen(false, nameof(IsFailure))]
    public T? User => Value;

    /// <summary>
    /// Represents a successful result.
    /// </summary>
    /// <returns>
    /// A <see cref="IdentityUserResult{T}"/> indicating a successful operation with no error message.
    /// </returns>
    /// <param name="user">The user associated with the successful result.</param>
    public new static IdentityUserResult<T> Success(T user) => new(true, user);

    /// <summary>
    /// Creates an IdentityUserResult indicating failure with an optional error message.
    /// </summary>
    /// <param name="errorMessage">An optional error message providing details about the failure.</param>
    /// <returns>A IdentityUserResult indicating failure.</returns>
    public new static IdentityUserResult<T> Failure(string? errorMessage = null) => new(false, null, errorMessage);
}
