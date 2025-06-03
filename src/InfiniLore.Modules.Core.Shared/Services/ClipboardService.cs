// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace InfiniLore.Modules.Core.Shared;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<ClipboardService>]
public class ClipboardService(IJSRuntime jsRuntime) {
    public ValueTask WriteTextAsync(string text) => jsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", text);
}
