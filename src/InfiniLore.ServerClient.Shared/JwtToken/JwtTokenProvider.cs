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
[InjectableService<JwtTokenProvider>(ServiceLifetime.Scoped)]
public class JwtTokenProvider(IJSRuntime jsRuntime, IHttpClientFactory clientFactory, ILogger<JwtTokenProvider> logger, JwtTokenEncoder encoder) {
    private const string StorageKey = "jwt_token";

    /// <summary>
    /// Save the JWT token securely along with its expiration timestamp.
    /// </summary>
    public async Task SaveTokenAsync(string token, DateTime expiresAt) 
        => await jsRuntime.InvokeVoidAsync("secureStorage.saveToken", StorageKey, token, expiresAt.ToString("o"));

    /// <summary>
    /// Retrieve the JWT token securely and check for validity.
    /// </summary>
    public async Task<string?> GetTokenAsync() {
        try {
            // Retrieve token record from IndexedDB
            var tokenRecord = await jsRuntime.InvokeAsync<JsTokenRecord>("secureStorage.getToken", StorageKey);
            logger.LogWarning("Token record retrieved: {TokenRecord}", tokenRecord.Value);
            
            if (tokenRecord.Value is not null) {
                // Check if ExpiresAt is available in the storage
                //      If no ExpiresAt, decode the token and extract expiration
                DateTime expiresAt = !string.IsNullOrEmpty(tokenRecord.ExpiresAt)
                    ? DateTime.Parse(tokenRecord.ExpiresAt!)
                    : encoder.GetTokenExpiry(tokenRecord.Value);

                // Validate token expiration
                if (DateTime.UtcNow < expiresAt) {
                    logger.LogWarning("Token is valid, returning stored token: {Token}", tokenRecord.Value);
                    return tokenRecord.Value;
                }

                logger.LogWarning("Token expired at {ExpiresAt}, fetching a new token.", expiresAt);
            }

            // Fetch a new token from the server if no valid token is found
            using HttpClient client = clientFactory.CreateClient("ServerAPI");
            string responseJson = await client.GetStringAsync("account/token");
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
            
            await SaveTokenAsync(response.Token, newExpiresAt);
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
        try {
            await jsRuntime.InvokeVoidAsync("secureStorage.removeToken", StorageKey);
        }
        catch (Exception e) {
            logger.Error(e, "Failed to remove token from secureStorage");
        }
    }
}
