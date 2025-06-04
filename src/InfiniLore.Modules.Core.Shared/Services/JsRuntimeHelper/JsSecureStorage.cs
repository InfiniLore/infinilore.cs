// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace InfiniLore.Modules.Core.Shared;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IJsSecureStorage>]
public class JsSecureStorage(
    ILogger<JsSecureStorage> logger,
    IJSRuntime jsRuntime
) : IJsSecureStorage{
    private const string JsSaveTokenAsync = "secureStorage.saveTokenAsync";
    private const string JsGetTokenAsync = "secureStorage.getTokenAsync";
    private const string JsRemoveTokenAsync = "secureStorage.removeTokenAsync";

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async Task SaveTokenAsync(string storageKey, string token,  DateTime expiresAt, CancellationToken ct = default) {
        try {
            await jsRuntime.InvokeVoidAsync(JsSaveTokenAsync, ct, storageKey, token, expiresAt.ToString("o"));
        }
        catch (Exception e) {
            logger.Error(e, "Failed to save token to secureStorage");
        }
    }
    
    public async Task<T?> GetTokenAsync<T>(string storageKey, CancellationToken ct) {
        try {
            return await jsRuntime.InvokeAsync<T>(JsGetTokenAsync, ct, storageKey);
        }
        catch (Exception e) {
            logger.Error(e, "Failed to retrieve token from secureStorage");
            return default;
        }
    }
    public async Task RemoveTokenAsync(string storageKey, CancellationToken ct = default) {
        try {
            await jsRuntime.InvokeVoidAsync(JsRemoveTokenAsync, ct, storageKey);
        }
        catch (Exception e) {
            logger.Error(e, "Failed to remove token from secureStorage");
        }
    }
}
