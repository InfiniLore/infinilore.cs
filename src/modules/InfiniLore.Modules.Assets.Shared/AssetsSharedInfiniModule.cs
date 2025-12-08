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
public class AssetsSharedInfiniModule : InfiniModule {
    protected override void OnConfiguring(IServiceCollection services) {
        services.RegisterServicesFromInfiniLoreModulesAssetsShared();
    }
    
    protected override void OnStartup(WebApplication app) {
        
    }
}
