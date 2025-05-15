// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Shared.Modules.LoreScopes;
using InfiniLore.Wasm.Modules.Core.Services;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Wasm.Modules.LoreScopes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class LoreScopeModuleSetup : WasmModuleSetup {
    public override void SetupServices(IServiceCollection services) {
        services.RegisterServicesFromInfiniLoreSharedModulesLoreScopes();
        services.RegisterServicesFromInfiniLoreWasmModulesLoreScopes();
    }
}
