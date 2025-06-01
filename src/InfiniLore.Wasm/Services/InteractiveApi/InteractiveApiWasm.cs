// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Kiota;
using InfiniLore.Wasm.Contracts.Services;

namespace InfiniLore.Wasm.Services.InteractiveApi;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IInteractiveApiWasm>]
public class InteractiveApiWasm(
    InfiniLoreApiClient apiClient
) : IInteractiveApiWasm {

    public InfiniLoreApiClient ApiClient { get; } = apiClient;
}
