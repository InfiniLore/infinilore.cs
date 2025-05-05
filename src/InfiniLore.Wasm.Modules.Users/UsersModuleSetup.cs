// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Shared.Modules.Users;
using InfiniLore.Wasm.Modules.Core.Services;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace InfiniLore.Wasm.Modules.Users;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class UsersModuleSetup : IWasmModuleSetup {

    public void Setup(WebAssemblyHostBuilder builder) {
        builder.Services.RegisterServicesFromInfiniLoreSharedModulesUsers();
        builder.Services.RegisterServicesFromInfiniLoreWasmModulesUsers();
    }
}
