// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace InfiniLore.Server.Services;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<ClipboardService>(ServiceLifetime.Scoped)]
public class ClipboardService(IJSRuntime jsRuntime) {
    public ValueTask WriteTextAsync(string text) => jsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", text);
}
