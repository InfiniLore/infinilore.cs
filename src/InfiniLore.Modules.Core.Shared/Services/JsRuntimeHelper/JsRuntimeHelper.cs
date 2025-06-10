// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System.Text.Json;

namespace InfiniLore.Modules.Core.Shared;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IJsRuntimeHelper>]
public class JsRuntimeHelper(
    ILogger<JsRuntimeHelper> logger,
    IJSRuntime jsRuntime,
    IJsSecureStorage jsSecureStorage
) : IJsRuntimeHelper {

    public IJsSecureStorage SecureStorage { get; } = jsSecureStorage;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async ValueTask WriteToClipboardAsync(string text) {
        try {
            await jsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", text);
        }
        catch (Exception e) {
            logger.Error(e, "Failed to write text to clipboard");
        }
    }
    
    public async ValueTask StoreToLocalStorageAsync<T>(string key, T value) {
        try {
            string json;
            if (value is bool boolValue) {
                // Store booleans as lowercase strings
                json = boolValue.ToString().ToLowerInvariant();
            }
            else {
                json = JsonSerializer.Serialize(value);
            }

            await jsRuntime.InvokeVoidAsync("localStorage.setItem", key, json);

        }
        catch (JSException e) {
            logger.LogError(e, "Error setting item to local storage");
        }
    }
    
    public async ValueTask<T?> GetFromLocalStorageAsync<T>(string key) {
        try {
            string json = await jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
            if (string.IsNullOrEmpty(json)) {
                return default;
            }

            // Special handling for boolean values
            if (typeof(T) != typeof(bool)) return JsonSerializer.Deserialize<T>(json)!;

            if (bool.TryParse(json, out bool result)) {
                return (T)(object)result;
            }

            return default;

        }
        catch (JSException e) {
            logger.LogError(e, "Error getting item from local storage");
            return default!;
        }
    }
    
    public async ValueTask RemoveFromLocalStorageAsync(string key) {
        try {
            await jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        }
        catch (JSException e) {
            logger.LogError(e, "Error removing item from local storage");
        }
    }
}
