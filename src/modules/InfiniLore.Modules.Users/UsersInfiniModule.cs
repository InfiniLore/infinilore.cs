// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Modular;
using InfiniLore.Modules.Users.Components;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Modules.Users;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class UsersInfiniModule : InfiniModule {

    protected override void OnModuleRegister(IServiceCollection services) {
        services.RegisterServicesFromInfiniLoreModulesUsers();
        
        AddSubModule<UsersSharedInfiniModule>();
        AddSubModule<UsersComponentsInfiniModule>();
    }
}
