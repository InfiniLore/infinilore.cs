// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Modular;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Modules.Users;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class UsersInfiniModule : InfiniModule {

    protected override void Configure() {
        Services.RegisterServicesFromInfiniLoreModulesUsers();
        
        AddSubModule<UsersSharedInfiniModule>();
    }
}
