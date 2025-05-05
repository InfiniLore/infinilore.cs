// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Kiota;
using InfiniLore.Wasm.Contracts.Services;
using System.Text.Json;

namespace InfiniLore.Wasm.Services;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IInteractiveApiWasm>]
public class InteractiveApiWasm(
    InfiniLoreApiClient apiClient
) : IInteractiveApiWasm {

    public InfiniLoreApiClient ApiClient { get; } = apiClient;
    public JsonSerializerOptions JsonOptions { get; } =  new() { PropertyNameCaseInsensitive = true };
}
