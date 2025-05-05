// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Shared;
using InfiniLore.Shared.Modules.Core;
using InfiniLore.Wasm.Modules.Core.Services;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Wasm.Modules.Core;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class CoreModuleSetup : IWasmModuleSetup {

    public void SetupBuilder(WebAssemblyHostBuilder builder) {
        
    }
    
    public void SetupServices(IServiceCollection services) {
        services.RegisterServicesFromInfiniLoreShared();
        services.RegisterServicesFromInfiniLoreSharedModulesCore();
        services.RegisterServicesFromInfiniLoreWasmModulesCore();
    }
}
