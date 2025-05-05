// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Modules.Core;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;

namespace InfiniLore.Server.Modules.MarkdownFiles;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class MarkdownFilesModuleSetup : IServerModuleSetup {

    public void Setup(WebApplicationBuilder builder) {
        builder.Services.RegisterServicesFromInfiniLoreServerModulesMarkdownFiles();
    }
}
