// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Clients.Wasm.Services.AuthenticationStateSyncer;
using InfiniLore.ServerClient.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Serilog;

namespace InfiniLore.Clients.Wasm;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class Program {
    public static async Task Main(string[] args) {
        // -------------------------------------------------------------------------------------------------------------
        // Builder
        // -------------------------------------------------------------------------------------------------------------
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.Services.AddAuthorizationCore();
        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

        builder.Services.AddHttpClient("ServerAPI", 
            client => client.BaseAddress = new Uri("https://localhost:7059/"));
        
        builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
            .CreateClient("ServerAPI"));

        builder.Services.AddHttpClient();

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()// You can adjust this to Information, Warning, Error, etc.
            .WriteTo.BrowserConsole()// Log to browser console for WASM
            .CreateLogger();

        builder.Logging.AddSerilog();
        builder.Services.RegisterServicesFromInfiniLoreServerClientShared();


        // -------------------------------------------------------------------------------------------------------------
        // App
        // -------------------------------------------------------------------------------------------------------------
        WebAssemblyHost app = builder.Build();
        await app.RunAsync();
    }
}
