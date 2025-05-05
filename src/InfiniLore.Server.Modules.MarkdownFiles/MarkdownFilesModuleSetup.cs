// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Modules.Core;
using InfiniLore.Shared.Modules.MarkdownFiles;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Server.Modules.MarkdownFiles;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class MarkdownFilesModuleSetup : IServerModuleSetup {

    public void SetupBuilder(WebApplicationBuilder builder) {
        
    }
    
    public void SetupServices(IServiceCollection services) {
        services.RegisterServicesFromInfiniLoreSharedModulesMarkdownFiles();
        services.RegisterServicesFromInfiniLoreServerModulesMarkdownFiles();
    }
}
