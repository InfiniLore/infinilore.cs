// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Modular;
using InfiniLore.Modules.Users.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Modules.Users;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class UsersSharedInfiniModule : InfiniModule {

    protected override void Configure() {
        Services.RegisterServicesFromInfiniLoreModulesUsersShared();
    }
}
