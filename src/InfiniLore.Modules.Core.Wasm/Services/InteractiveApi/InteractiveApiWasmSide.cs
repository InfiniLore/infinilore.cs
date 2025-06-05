// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Kiota;

namespace InfiniLore.Modules.Core.Wasm.Services.InteractiveApi;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IInteractiveApiWasm>]
internal class InteractiveApiWasm(
    InfiniLoreApiClient apiClient
) : IInteractiveApiWasm {
    public InfiniLoreApiClient ApiClient { get; } = apiClient;
    
    public string DefaultApiError => "Could not get data from API";
}
