// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Modules.Core;
using InfiniLore.Shared.Modules.MarkdownFiles;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;

namespace InfiniLore.Server.Modules.MarkdownFiles;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class MarkdownFilesModuleSetup : IServerModuleSetup {

    public void Setup(WebApplicationBuilder builder) {
        builder.Services.RegisterServicesFromInfiniLoreSharedModulesMarkdownFiles();
        builder.Services.RegisterServicesFromInfiniLoreServerModulesMarkdownFiles();
    }
}
