// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Shared;
using InfiniLore.Shared.Modules.Core;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Server.Modules.Core;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class CoreModuleSetup : ServerModuleSetup {
    public override void SetupServices(IServiceCollection services) {
        services.RegisterServicesFromInfiniLoreShared();
        services.RegisterServicesFromInfiniLoreSharedModulesCore();
        services.RegisterServicesFromInfiniLoreServerModulesCore();
    }
}
