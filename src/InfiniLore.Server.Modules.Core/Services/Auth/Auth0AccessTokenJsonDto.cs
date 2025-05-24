// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Credentials.Auth0;
using System.Text.Json.Serialization;

namespace InfiniLore.Server.Modules.Core.Auth;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record Auth0AccessTokenJsonDto (
    [property: JsonPropertyName("token")] string Token,
    [property: JsonPropertyName("expires_at")] DateTime ExpiresAt,
    [property: JsonPropertyName("scopes")] string? Scopes,
    [property: JsonPropertyName("token_type")] string? TokenType
) : IAuth0AccessToken  {
    public bool IsAccessTokenOnly => false;

    public bool IsExpired => ExpiresAt < DateTime.UtcNow;
    public bool IsExpiredIn5Minutes => ExpiresAt < DateTime.UtcNow.AddMinutes(5);
    public bool IsEmpty => Token.IsNullOrWhiteSpace();
    public bool IsNotEmpty => !IsEmpty;

    public static Auth0AccessTokenJsonDto FromToken(IAuth0AccessToken accessToken) =>
        new(accessToken.Token, accessToken.ExpiresAt, accessToken.Scopes, accessToken.TokenType);
}
