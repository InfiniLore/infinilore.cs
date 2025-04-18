// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System.Text.Json;

namespace InfiniLore.ServerClient.Shared.JwtToken;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IJwtTokenJsSecureStorage>(ServiceLifetime.Scoped)]
public class JwtTokenJsSecureStorage(IJSRuntime jsRuntime, IHttpClientFactory clientFactory, ILogger<JwtTokenJsSecureStorage> logger, IJwtTokenEncoder encoder) : IJwtTokenJsSecureStorage {
    private const string StorageKey = "jwt_token";

    private const string JsSaveTokenAsync = "secureStorage.saveTokenAsync";
    private const string JsGetTokenAsync = "secureStorage.getTokenAsync";
    private const string JsRemoveTokenAsync = "secureStorage.removeTokenAsync";

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Save the JWT token securely along with its expiration timestamp.
    /// </summary>
    public async Task SaveTokenAsync(string token, DateTime expiresAt, CancellationToken ct = default) {
        try {
            await jsRuntime.InvokeVoidAsync(JsSaveTokenAsync, ct, StorageKey, token, expiresAt.ToString("o"));
        }
        catch (Exception e) {
            logger.Error(e, "Failed to save token to secureStorage");
        }
    }

    /// <summary>
    ///     Retrieve the JWT token securely and check for validity.
    /// </summary>
    public async Task<string?> GetTokenAsync(CancellationToken ct = default) {
        try {
            // Retrieve token record from IndexedDB
            var tokenRecord = await jsRuntime.InvokeAsync<JsTokenRecord?>(JsGetTokenAsync, ct, StorageKey);
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

                await SaveTokenAsync(tokenRecord.Value, expiresAt, ct);
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
        try {
            await jsRuntime.InvokeVoidAsync(JsRemoveTokenAsync, ct, StorageKey);
        }
        catch (Exception e) {
            logger.Error(e, "Failed to remove token from secureStorage");
        }
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

            await SaveTokenAsync(response.Token, newExpiresAt, ct);
            return response.Token;
        }
        catch (Exception ex) {
            logger.LogError(ex, "Failed to retrieve token from secureStorage or server.");
            return null;
        }
    }
}
