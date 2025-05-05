// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Shared.Modules.LoreScopes;
using InfiniLore.Wasm.Modules.Core.Services;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace InfiniLore.Wasm.Modules.LoreScopes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class LoreScopeModuleSetup : IWasmModuleSetup {

    public void Setup(WebAssemblyHostBuilder builder) {
        builder.Services.RegisterServicesFromInfiniLoreSharedModulesLoreScopes();
        builder.Services.RegisterServicesFromInfiniLoreWasmModulesLoreScopes();
    }
}
