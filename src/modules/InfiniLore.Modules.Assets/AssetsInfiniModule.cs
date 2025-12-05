// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Modular;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Modules.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class AssetsInfiniModule : InfiniModule {
    protected override void OnConfiguring(IServiceCollection services) {
        services.RegisterServicesFromInfiniLoreModulesAssets();
        
        AddSubModule<AssetsSharedInfiniModule>();
    }
    
    protected override void OnStartup(WebApplication app) {
        
    }
}
