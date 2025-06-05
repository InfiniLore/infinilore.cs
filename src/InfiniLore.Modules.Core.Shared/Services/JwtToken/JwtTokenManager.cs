// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace InfiniLore.Modules.Core.Shared;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IJwtTokenManager>]
public class JwtTokenManager(
    IJsRuntimeHelper jsRuntimeHelper,
    IHttpClientFactory clientFactory,
    ILogger<JwtTokenManager> logger,
    IJwtTokenEncoder encoder
) : IJwtTokenManager {
    private const string StorageKey = "jwt_token";

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Retrieve the JWT token securely and check for validity.
    /// </summary>
    public async Task<string?> GetTokenAsync(CancellationToken ct = default) {
        try {
            // Retrieve token record from IndexedDB
            var tokenRecord = await jsRuntimeHelper.SecureStorage.GetTokenAsync<JsTokenRecord?>(StorageKey, ct);
            if (tokenRecord?.Value is null) {
                logger.LogInformation("No token found in storage, fetching a new token.");
                return await RetrieveAndStoreTokenAsync(ct);
            }

            // Check if ExpiresAt is available in the storage
            //      If no ExpiresAt, decode the token and extract expiration
            if (!DateTime.TryParse(tokenRecord.ExpiresAt, out DateTime expiresAt)) {
                logger.LogInformation("No token expiry found in storage, decoding token to extract expiry.");
                if (!encoder.TryGetTokenUtcExpiry(tokenRecord.Value, out expiresAt)) {
                    logger.LogInformation("Failed to extract token expiry from token, fetching a new token.");
                    return await RetrieveAndStoreTokenAsync(ct);
                }

                await jsRuntimeHelper.SecureStorage.SaveTokenAsync(StorageKey, tokenRecord.Value, expiresAt, ct);
            }

            // ReSharper disable once InvertIf
            if (DateTime.UtcNow >= expiresAt) {
                logger.LogInformation("Token expired at {ExpiresAt}, fetching a new token.", expiresAt);
                return await RetrieveAndStoreTokenAsync(ct);
            }

            return tokenRecord.Value;
        }
        catch (Exception ex) {
            logger.LogError(ex, "Failed to retrieve token");
            return null;
        }
    }

    /// <summary>
    ///     Remove the JWT token securely (e.g., during logout).
    /// </summary>
    public async Task RemoveTokenAsync(CancellationToken ct = default) {
        await jsRuntimeHelper.SecureStorage.RemoveTokenAsync(StorageKey, ct);
    }

    private async Task<string?> RetrieveAndStoreTokenAsync(CancellationToken ct = default) {
        try {
            // Fetch a new token from the server if no valid token is found
            using HttpClient client = clientFactory.CreateClient("ServerAPI");
            string responseJson = await client.GetStringAsync("account/token", ct);
            if (JsonSerializer.Deserialize<TokenResponse>(responseJson) is not {} response) {
                logger.LogError("Failed to deserialize token response: {ResponseJson}", responseJson);
                return null;
            }

            if (response.Token.IsNullOrEmpty()) {
                logger.LogError("Failed to retrieve token from response: {ResponseJson}", responseJson);
                return null;
            }

            if (!DateTime.TryParse(response.ExpiresAt, out DateTime newExpiresAt)) {
                logger.LogError("Failed to parse token expiry from response: {ResponseJson}", responseJson);
                return null;
            }

            await jsRuntimeHelper.SecureStorage.SaveTokenAsync(StorageKey, response.Token, newExpiresAt, ct);
            return response.Token;
        }
        catch (Exception ex) {
            logger.LogError(ex, "Failed to retrieve token from secureStorage or server.");
            return null;
        }
    }
}
