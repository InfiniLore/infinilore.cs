// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.LoreScopes.Shared;
using InfiniLore.Modules.LoreScopes.Shared.Components;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace InfiniLore.Modules.LoreScopes.Server;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class ModuleSetupLoreScopesServer : IModuleSetup {
    public Assembly ComponentAssembly => IComponentsEntryLoreScopes.Assembly;
    
    public void SetupServices(IServiceCollection services) {
        services.RegisterServicesFromInfiniLoreModulesLoreScopesShared();
        services.RegisterServicesFromInfiniLoreModulesLoreScopesServer();
    }
}
