// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Shared.Modules.MarkdownFiles;
using InfiniLore.Wasm.Modules.Core.Services;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Wasm.Modules.MarkdownFiles;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class MarkdownFilesModuleSetup : WasmModuleSetup {
    public override void SetupServices(IServiceCollection services) {
        services.RegisterServicesFromInfiniLoreSharedModulesMarkdownFiles();
        services.RegisterServicesFromInfiniLoreWasmModulesMarkdownFiles();
    }
}
