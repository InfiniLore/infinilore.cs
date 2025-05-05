// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Shared.Modules.MarkdownFiles;
using InfiniLore.Wasm.Modules.Core.Services;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Wasm.Modules.MarkdownFiles;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class MarkdownFilesModuleSetup : IWasmModuleSetup {

    public void SetupBuilder(WebAssemblyHostBuilder builder) {
        
    }
    
    public void SetupServices(IServiceCollection services) {
        services.RegisterServicesFromInfiniLoreSharedModulesMarkdownFiles();
        services.RegisterServicesFromInfiniLoreWasmModulesMarkdownFiles();
    }
}
