// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Shared.Modules.Core;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Server.Modules.Core;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class CoreModuleSetup : IServerModuleSetup {

    public void SetupBuilder(WebApplicationBuilder builder) {
        
    }
    
    public void SetupServices(IServiceCollection services) {
       services.RegisterServicesFromInfiniLoreSharedModulesCore();
       services.RegisterServicesFromInfiniLoreServerModulesCore();
    }
}
