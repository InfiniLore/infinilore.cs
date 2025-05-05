// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Shared.Modules.MarkdownFiles;
using InfiniLore.Wasm.Modules.Core.Services;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace InfiniLore.Wasm.Modules.MarkdownFiles;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class MarkdownFilesModuleSetup : IWasmModuleSetup {

    public void Setup(WebAssemblyHostBuilder builder) {
        builder.Services.RegisterServicesFromInfiniLoreSharedModulesMarkdownFiles();
        builder.Services.RegisterServicesFromInfiniLoreWasmModulesMarkdownFiles();
    }
}
