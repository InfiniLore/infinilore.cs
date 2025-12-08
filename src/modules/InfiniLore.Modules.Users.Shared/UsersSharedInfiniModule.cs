// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Modular;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Modules.Users;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class UsersSharedInfiniModule : InfiniModule {

    protected override void OnConfiguring(IServiceCollection services) {
        services.RegisterServicesFromInfiniLoreModulesUsersShared();
    }
    
    protected override void OnStartup(WebApplication app) {
        
    }
}
