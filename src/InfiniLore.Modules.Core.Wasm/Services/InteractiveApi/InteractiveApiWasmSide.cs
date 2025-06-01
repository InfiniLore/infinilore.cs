// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Kiota;
using InfiniLore.Modules.Core.Wasm.Contracts.Services;

namespace InfiniLore.Modules.Core.Wasm.Services.InteractiveApi;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IInteractiveApiWasm>]
internal class InteractiveApiWasm(
    InfiniLoreApiClient apiClient
) : IInteractiveApiWasm {
    public InfiniLoreApiClient ApiClient { get; } = apiClient;
    
    public string DefaultApiError { get; } = "Could not get data from API";
}
