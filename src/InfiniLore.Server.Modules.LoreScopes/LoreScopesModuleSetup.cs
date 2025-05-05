// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Modules.Core;
using InfiniLore.Shared.Modules.LoreScopes;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Server.Modules.LoreScopes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class LoreScopesModuleSetup : IServerModuleSetup {

    public void SetupBuilder(WebApplicationBuilder builder) {
        
    }
    
    public void SetupServices(IServiceCollection services) {
        services.RegisterServicesFromInfiniLoreSharedModulesLoreScopes();
        services.RegisterServicesFromInfiniLoreServerModulesLoreScopes();
    }
}
