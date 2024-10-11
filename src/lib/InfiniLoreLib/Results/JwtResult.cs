// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

namespace InfiniLoreLib.Results;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
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
    
    private new bool IsSuccess => base.IsSuccess;

    [MemberNotNullWhen(true, nameof(IsSuccess))]
    public string? AccessToken => Value.AccessToken;
    [MemberNotNullWhen(true, nameof(IsSuccess))]
    public DateTime? AccessTokenExpiryUtc => Value.AccessTokenExpiryUTC;
    [MemberNotNullWhen(true, nameof(IsSuccess))]
    public Guid? RefreshToken => Value.RefreshToken;
    [MemberNotNullWhen(true, nameof(IsSuccess))]
    public DateTime? RefreshTokenExpiryUtc => Value.RefreshTokenExpiryUTC;
    
    public static JwtResult Success(string accessToken, Guid refreshToken, DateTime accessTokenExpiryUtc, DateTime refreshTokenExpiryUtc) =>
        new(true, accessToken, accessTokenExpiryUtc, refreshToken, refreshTokenExpiryUtc);
    public new static JwtResult Failure(string? errorMessage = null) => new(false, ErrorMessage:errorMessage);
}
