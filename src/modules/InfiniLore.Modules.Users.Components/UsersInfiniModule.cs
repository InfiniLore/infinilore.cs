// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Modular;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Modules.Users.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class UsersComponentsInfiniModule : InfiniModule {

    protected override void OnConfiguring(IServiceCollection services) {
        services.RegisterServicesFromInfiniLoreModulesUsersComponents();
    }
    
    protected override void OnStartup(WebApplication app) {
        
    }
}
