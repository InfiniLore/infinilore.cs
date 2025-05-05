// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Modules.Core;
using InfiniLore.Shared.Modules.Users;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;

namespace InfiniLore.Server.Modules.Users;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class UsersModuleSetup : IServerModuleSetup {

    public void Setup(WebApplicationBuilder builder) {
        builder.Services.RegisterServicesFromInfiniLoreSharedModulesUsers();
        builder.Services.RegisterServicesFromInfiniLoreServerModulesUsers();
    }
}
