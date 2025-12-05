// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Modular;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Modules.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class AssetsInfiniModule : InfiniModule {
    protected override void OnModuleRegister(IServiceCollection services) {
        services.RegisterServicesFromInfiniLoreModulesAssets();
        
        AddSubModule<AssetsSharedInfiniModule>();
    }
}
