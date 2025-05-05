// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Modules.Core;
using InfiniLore.Shared.Modules.Users;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Server.Modules.Users;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class UsersModuleSetup : IServerModuleSetup {

    public void SetupBuilder(WebApplicationBuilder builder) {
        
    }
    
    public void SetupServices(IServiceCollection services) {
        services.RegisterServicesFromInfiniLoreSharedModulesUsers();
        services.RegisterServicesFromInfiniLoreServerModulesUsers();
    }
}
