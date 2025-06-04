// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Shared;
using InfiniLore.Modules.Core.Wasm.Contracts;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Modules.Core.Wasm;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class CoreModuleSetup : WasmModuleSetup {
    public override void SetupServices(IServiceCollection services) {
        services.RegisterServicesFromInfiniLoreModulesCoreShared();
        services.RegisterServicesFromInfiniLoreModulesCoreWasm();
    }
}
