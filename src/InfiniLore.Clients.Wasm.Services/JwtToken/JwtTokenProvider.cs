// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace InfiniLore.Clients.Wasm.Services.JwtToken;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class JwtTokenProvider(IJSRuntime jsRuntime, IHttpClientFactory clientFactory, ILogger<JwtTokenProvider> logger) {
    private const string StorageKey = "jwt_token";

    /// <summary>
    ///     Save the JWT token securely.
    /// </summary>
    public async Task SaveTokenAsync(string token) {
        await jsRuntime.InvokeVoidAsync("secureStorage.saveToken", StorageKey, token);
    }

    /// <summary>
    ///     Retrieve the JWT token securely.
    /// </summary>
    public async Task<string?> GetTokenAsync() {
        try {
            var tokenRecord = await jsRuntime.InvokeAsync<JsTokenRecord?>("secureStorage.getToken", StorageKey);

            if (tokenRecord?.Value is not null) {
                logger.LogDebug("Retrieved token from secure storage: {Token}", tokenRecord.Value);
                return tokenRecord.Value;
            }

            using HttpClient client = clientFactory.CreateClient("ServerAPI");
            string newToken = await client.GetStringAsync("account/token");
            await SaveTokenAsync(newToken);
            return newToken;
        }
        catch (Exception e) {
            logger.LogError(e, "Failed to get token from server");
            return null;
        }
    }
    
    /// <summary>
    ///     Remove the JWT token securely (e.g., during logout).
    /// </summary>
    public async Task RemoveTokenAsync() {
        await jsRuntime.InvokeVoidAsync("secureStorage.removeToken", StorageKey);
    }
    
    [UsedImplicitly] private class JsTokenRecord {
        public string Id { get; set; }
        public string? Value { get; set; }
    }

}
