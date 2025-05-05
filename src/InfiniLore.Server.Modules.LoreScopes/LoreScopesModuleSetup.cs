// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Modules.Core;
using InfiniLore.Shared.Modules.LoreScopes;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;

namespace InfiniLore.Server.Modules.LoreScopes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class LoreScopesModuleSetup : IServerModuleSetup {

    public void Setup(WebApplicationBuilder builder) {
        builder.Services.RegisterServicesFromInfiniLoreSharedModulesLoreScopes();
        builder.Services.RegisterServicesFromInfiniLoreServerModulesLoreScopes();
    }
}
