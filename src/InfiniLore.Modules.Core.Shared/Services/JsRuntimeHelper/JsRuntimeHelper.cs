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
}
