// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core;
using InfiniLore.InfiniFrame.Blazor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace InfiniLore.Application.Desktop;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class Program {
    [STAThread]
    public static void Main(string[] args) {
        // -------------------------------------------------------------------------------------------------------------
        // Builder
        // -------------------------------------------------------------------------------------------------------------
        var appBuilder = InfiniFrameBlazorAppBuilder.CreateDefault(args);
        
        appBuilder.Services.AddLogging(config => {
            config.ClearProviders();
            config.AddSerilog();
        });
        
        appBuilder.Services.AddSerilog(config => {
            config.WriteTo.Async(static c => c.Console())
                .MinimumLevel.Debug();
        });
        
        appBuilder.RootComponents.Add<App>("app");
        appBuilder.Services.AddInfiniBlazor(config => config.Components.SetRenderMode(null!));

        // -------------------------------------------------------------------------------------------------------------
        // Application
        // -------------------------------------------------------------------------------------------------------------
        InfiniFrameBlazorApp app = appBuilder.Build();

        app.Run();
    }
}
