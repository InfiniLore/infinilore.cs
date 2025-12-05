// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Modular;
using InfiniLore.Modules.Users.Components;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Modules.Users;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class UsersInfiniModule : InfiniModule {

    protected override void OnConfiguring(IServiceCollection services) {
        services.RegisterServicesFromInfiniLoreModulesUsers();
        
        AddSubModule<UsersSharedInfiniModule>();
        AddSubModule<UsersComponentsInfiniModule>();
    }

    protected override void OnStartup(WebApplication app) {
        app.Use(UserOnboarding.Middleware);
    }
}
