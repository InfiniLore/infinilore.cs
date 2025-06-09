// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.LsMarkdownFiles.Shared;
using InfiniLore.Modules.LsMarkdownFiles.Shared.Components;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace InfiniLore.Modules.LsMarkdownFiles.Wasm;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class ModuleSetupLsMarkdownFilesWasm : IModuleSetup {
    public Assembly ComponentAssembly => IComponentsEntryLsMarkdownFiles.Assembly;
    
    public void SetupServices(IServiceCollection services) {
        services.RegisterServicesFromInfiniLoreModulesLsMarkdownFilesShared();
        services.RegisterServicesFromInfiniLoreModulesLsMarkdownFilesWasm();
    }
}
