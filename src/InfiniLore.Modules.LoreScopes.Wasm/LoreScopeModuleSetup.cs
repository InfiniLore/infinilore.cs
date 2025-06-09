// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.LoreScopes.Shared;
using InfiniLore.Modules.LoreScopes.Shared.Components;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace InfiniLore.Modules.LoreScopes.Wasm;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class ModuleSetupLoreScopesWasm : IModuleSetup {
    public Assembly ComponentAssembly => IComponentsEntryLoreScopes.Assembly;
    
    public void SetupServices(IServiceCollection services) {
        services.RegisterServicesFromInfiniLoreModulesLoreScopesShared();
        services.RegisterServicesFromInfiniLoreModulesLoreScopesWasm();
    }
}
