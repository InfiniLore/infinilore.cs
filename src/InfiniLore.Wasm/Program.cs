// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Kiota;
using InfiniLore.Kiota.Extensions;
using InfiniLore.Wasm.Services.AuthenticationStateSyncer;
using InfiniLore.InfiniBlazor.Markdown.Config;
using InfiniLore.Shared;
using InfiniLore.Wasm.Modules.Core.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Serilog;
using CoreAssemblyEntry = InfiniLore.Wasm.Modules.Core.IAssemblyEntry;
using LoreScopesAssemblyEntry = InfiniLore.Wasm.Modules.LoreScopes.IAssemblyEntry;
using MarkdownFilesAssemblyEntry = InfiniLore.Wasm.Modules.MarkdownFiles.IAssemblyEntry;
using UsersAssemblyEntry = InfiniLore.Wasm.Modules.Users.IAssemblyEntry;

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
        
        WasmModuleBuilder moduleBuilder = WasmModuleBuilder.CreateFromBuilder(builder)
            .AddModule<CoreAssemblyEntry>()
            .AddModule<LoreScopesAssemblyEntry>()
            .AddModule<MarkdownFilesAssemblyEntry>()
            .AddModule<UsersAssemblyEntry>();
        
        builder.Services.AddAuthorizationCore();
        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddSingleton<AuthenticationStateProvider, WasmClientAuthenticationStateProvider>();

        builder.Services.AddHttpClient();
        builder.Services.RegisterServicesFromInfiniLoreShared();
        builder.Services.RegisterServicesFromInfiniLoreWasm();

        builder.Services.AddKiotaHandlers();
        builder.Services.AddHttpClient<InfiniLoreApiClientFactory>("ServerAPI",
            configureClient: static client => client.BaseAddress = new Uri("https://localhost:7059/")
        ).AttachKiotaHandlers();

        builder.Services.AddTransient<InfiniLoreApiClient>(static sp => sp.GetRequiredService<InfiniLoreApiClientFactory>().GetClient());
            
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
