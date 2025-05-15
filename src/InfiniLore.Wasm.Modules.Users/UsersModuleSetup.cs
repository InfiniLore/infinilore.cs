// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Shared.Modules.Users;
using InfiniLore.Wasm.Modules.Core.Services;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Wasm.Modules.Users;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class UsersModuleSetup : WasmModuleSetup {
    public override void SetupServices(IServiceCollection services) {
        services.RegisterServicesFromInfiniLoreSharedModulesUsers();
        services.RegisterServicesFromInfiniLoreWasmModulesUsers();
    }
}
