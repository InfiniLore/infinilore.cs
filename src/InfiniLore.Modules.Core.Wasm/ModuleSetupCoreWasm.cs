// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Shared;
using InfiniLore.Modules.Core.Shared.Components;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace InfiniLore.Modules.Core.Wasm;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class ModuleSetupCoreWasm : IModuleSetup {
    public Assembly ComponentAssembly => IComponentsEntryCore.Assembly;
    
    public void SetupServices(IServiceCollection services) {
        services.RegisterServicesFromInfiniLoreModulesCoreShared();
        services.RegisterServicesFromInfiniLoreModulesCoreWasm();
    }
}
