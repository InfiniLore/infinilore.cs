// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Shared.Modules.Users;
using InfiniLore.Wasm.Modules.Core.Services;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Wasm.Modules.Users;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class UsersModuleSetup : IWasmModuleSetup {

    public void SetupBuilder(WebAssemblyHostBuilder builder) {
        
    }
    
    public void SetupServices(IServiceCollection services) {
        services.RegisterServicesFromInfiniLoreSharedModulesUsers();
        services.RegisterServicesFromInfiniLoreWasmModulesUsers();
    }
}
