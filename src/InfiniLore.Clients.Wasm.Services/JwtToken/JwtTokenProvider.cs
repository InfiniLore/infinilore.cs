// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.ServerClient.Shared;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System.Text.Json;

namespace InfiniLore.Clients.Wasm.Services.JwtToken;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class JwtTokenProvider(IJSRuntime jsRuntime, IHttpClientFactory clientFactory, ILogger<JwtTokenProvider> logger, JwtTokenEncoder encoder) {
    private const string StorageKey = "jwt_token";

    /// <summary>
    /// Save the JWT token securely along with its expiration timestamp.
    /// </summary>
    public async Task SaveTokenAsync(string token, DateTime expiresAt) {
        await jsRuntime.InvokeVoidAsync("secureStorage.saveToken", StorageKey, token, expiresAt.ToString("o"));
    }

    /// <summary>
    /// Retrieve the JWT token securely and check for validity.
    /// </summary>
    public async Task<string?> GetTokenAsync() {
        try {
            // Retrieve token record from IndexedDB
            var tokenRecord = await jsRuntime.InvokeAsync<JsTokenRecord?>("secureStorage.getToken", StorageKey);

            if (tokenRecord?.Value is not null) {
                // Check if ExpiresAt is available in the storage
                //      If no ExpiresAt, decode the token and extract expiration
                DateTime expiresAt = !string.IsNullOrEmpty(tokenRecord.ExpiresAt)
                    ? DateTime.Parse(tokenRecord.ExpiresAt!)
                    : encoder.GetTokenExpiry(tokenRecord.Value);

                // Validate token expiration
                if (DateTime.UtcNow < expiresAt) {
                    logger.LogDebug("Token is valid, returning stored token: {Token}", tokenRecord.Value);
                    return tokenRecord.Value;
                }

                logger.LogDebug("Token expired at {ExpiresAt}, fetching a new token.", expiresAt);
            }

            // Fetch a new token from the server if no valid token is found
            using HttpClient client = clientFactory.CreateClient("ServerAPI");
            string responseJson = await client.GetStringAsync("account/token");
            var response = JsonSerializer.Deserialize<TokenResponse>(responseJson);

            if (response is null) return null;

            await SaveTokenAsync(response.Token, response.ExpiresAt);
            return response.Token;
        }
        catch (Exception e) {
            logger.LogError(e, "Failed to retrieve token");
            return null;
        }
    }

    /// <summary>
    /// Remove the JWT token securely (e.g., during logout).
    /// </summary>
    public async Task RemoveTokenAsync() {
        await jsRuntime.InvokeVoidAsync("secureStorage.removeToken", StorageKey);
    }
    
    // Store token and its expiration info
    [UsedImplicitly] private class JsTokenRecord {
        [UsedImplicitly] public string Id { get; set; } = null!;
        [UsedImplicitly] public string? Value { get; set; }
        [UsedImplicitly] public string? ExpiresAt { get; set; } // ISO 8601 formatted expiration timestamp
    }

    // Server response format
    private class TokenResponse {
        public string Token { get; init; } = null!;
        public DateTime ExpiresAt { get; init; }
    }
}
