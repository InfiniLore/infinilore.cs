// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Modules.Core;
using InfiniLore.Shared.Modules.Users;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Server.Modules.Users;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class UsersModuleSetup : ServerModuleSetup {
    public override void SetupServices(IServiceCollection services) {
        services.RegisterServicesFromInfiniLoreSharedModulesUsers();
        services.RegisterServicesFromInfiniLoreServerModulesUsers();
    }
}
