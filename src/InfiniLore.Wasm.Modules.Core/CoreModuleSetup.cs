// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Shared.Modules.Core;
using InfiniLore.Wasm.Modules.Core.Services;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace InfiniLore.Wasm.Modules.Core;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class CoreModuleSetup : IWasmModuleSetup {

    public void Setup(WebAssemblyHostBuilder builder) {
        builder.Services.RegisterServicesFromInfiniLoreSharedModulesCore();
        builder.Services.RegisterServicesFromInfiniLoreWasmModulesCore();
    }
}
