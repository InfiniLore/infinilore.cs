// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.InfiniBlazor.Markdown.Config;
using InfiniLore.Kiota;
using InfiniLore.Modules.Core.Shared;
using InfiniLore.Modules.Core.Wasm;
using InfiniLore.Modules.Core.Wasm.Services;
using InfiniLore.Modules.LoreScopes.Wasm;
using InfiniLore.Modules.LsMarkdownFiles.Wasm;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Serilog;

namespace InfiniLore.Wasm;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class Program {
    public static async Task Main(string[] args) {
        // -------------------------------------------------------------------------------------------------------------
        // Builder
        // -------------------------------------------------------------------------------------------------------------
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.BrowserConsole()// Log to browser console for WASM
            .CreateLogger();

        builder.Logging.AddSerilog();

        WasmModuleBuilder _ = WasmModuleBuilder.Create(builder)
            .AddModule<IWasmModuleEntryCore>()
            .AddModule<IModuleLoreScopesWasm>()
            .AddModule<IModulelsLsMarkdownFilesWasm>();
        
        builder.Services.AddAuthorizationCore();
        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddSingleton<AuthenticationStateProvider, AuthenticationStateProviderWasm>();

        builder.Services.AddHttpClient();
        builder.Services.RegisterServicesFromInfiniLoreWasm();
        builder.Services.RegisterServicesFromInfiniLoreModulesCoreShared();

        builder.Services.AddInfiniLoreKiotaClient();
            
        #region InfiniBlazor
        builder.Services.AddInfiniBlazor(config => {
            config.AddMarkdownLogic(markdownConfig => markdownConfig.AddMarkdownParser<string, string>());
        });
        #endregion
        // -------------------------------------------------------------------------------------------------------------
        // App
        // -------------------------------------------------------------------------------------------------------------
        WebAssemblyHost app = builder.Build();
        await app.RunAsync();
    }
}
