// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Server;
using InfiniLore.Modules.LsMarkdownFiles.Shared;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Modules.LsMarkdownFiles.Server;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class LsMarkdownFilesModuleSetup : ServerModuleSetup {
    public override void SetupServices(IServiceCollection services) {
        services.RegisterServicesFromInfiniLoreModulesLsMarkdownFilesShared();
        services.RegisterServicesFromInfiniLoreModulesLsMarkdownFilesServer();
    }
}
