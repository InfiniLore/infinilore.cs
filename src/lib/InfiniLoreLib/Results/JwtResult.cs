// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

namespace InfiniLoreLib.Results;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents the result of a JWT (JSON Web Token) operation, encapsulating the success status,
/// access token, refresh token, their expiry times, and any potential error message.
/// </summary>
public record JwtResult(
    bool IsSuccess,
    string? AccessToken = default,
    DateTime? AccessTokenExpiryUtc = default,
    Guid? RefreshToken = default,
    DateTime? RefreshTokenExpiryUtc = default,
    string? ErrorMessage = null
) : Result<(string? AccessToken, Guid? RefreshToken, DateTime? AccessTokenExpiryUTC, DateTime? RefreshTokenExpiryUTC)>(
    IsSuccess,
    (AccessToken, RefreshToken, AccessTokenExpiryUtc, RefreshTokenExpiryUtc),
    ErrorMessage
) {

    /// <summary>
    /// Indicates the success status of the JWT operation.
    /// </summary>
    private new bool IsSuccess => base.IsSuccess;

    /// <summary>
    /// Gets the access token that is used for authenticating user requests.
    /// If <see cref="IsSuccess"/> is true, this property contains a valid JWT access token.
    /// If <see cref="IsSuccess"/> is false, this property is null.
    /// </summary>
    [MemberNotNullWhen(true, nameof(IsSuccess))]
    public string? AccessToken => Value.AccessToken;
    
    /// <summary>
    /// Gets the UTC expiry date and time of the access token.
    /// </summary>
    /// <remarks>
    /// This property indicates when the access token will expire in Coordinated Universal Time (UTC).
    /// </remarks>
    [MemberNotNullWhen(true, nameof(IsSuccess))]
    public DateTime? AccessTokenExpiryUtc => Value.AccessTokenExpiryUTC;
    
    /// <summary>
    /// Gets the refresh token associated with the JWT result.
    /// </summary>
    /// <remarks>
    /// The refresh token can be used to obtain a new access token when the current access token expires.
    /// </remarks>
    [MemberNotNullWhen(true, nameof(IsSuccess))]
    public Guid? RefreshToken => Value.RefreshToken;
    
    /// <summary>
    /// Gets the UTC expiration date and time of the refresh token.
    /// </summary>
    /// <value>
    /// The expiration date and time of the refresh token in UTC.
    /// </value>
    [MemberNotNullWhen(true, nameof(IsSuccess))]
    public DateTime? RefreshTokenExpiryUtc => Value.RefreshTokenExpiryUTC;

    /// <summary>
    /// Creates a successful JwtResult instance.
    /// </summary>
    /// <param name="accessToken">The access token.</param>
    /// <param name="refreshToken">The refresh token.</param>
    /// <param name="accessTokenExpiryUtc">The access token expiry time in UTC.</param>
    /// <param name="refreshTokenExpiryUtc">The refresh token expiry time in UTC.</param>
    /// <returns>A successful JwtResult containing the provided tokens and expiry times.</returns>
    public static JwtResult Success(string accessToken, Guid refreshToken, DateTime accessTokenExpiryUtc, DateTime refreshTokenExpiryUtc) =>
        new(true, accessToken, accessTokenExpiryUtc, refreshToken, refreshTokenExpiryUtc);
    
    /// <summary>
    /// Creates a new <see cref="JwtResult"/> indicating failure.
    /// </summary>
    /// <param name="errorMessage">The error message detailing the reason for failure. Default is null.</param>
    /// <returns>A <see cref="JwtResult"/> instance where <see cref="JwtResult.IsSuccess"/> is false and <paramref name="errorMessage"/> is set.</returns>
    public new static JwtResult Failure(string? errorMessage = null) => new(false, ErrorMessage: errorMessage);
}
