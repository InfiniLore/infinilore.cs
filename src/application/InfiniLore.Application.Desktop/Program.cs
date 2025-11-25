// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using FastEndpoints.Swagger;
using InfiniFrame;
using InfiniFrame.Js;
using InfiniFrame.Js.MessageHandlers;
using InfiniFrame.WebServer;
using InfiniLore.Application.Desktop.Components;
using InfiniLore.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
        InfiniFrameWebApplicationBuilder applicationBuilder = InfiniFrameWebApplication.CreateBuilder(args);
        WebApplicationBuilder webAppBuilder = applicationBuilder.WebApp;

        webAppBuilder.Services.AddInfiniLoreApplication();
        
        webAppBuilder.Services.AddLogging(config => {
            config.ClearProviders();
            config.AddSerilog();
        });
        
        webAppBuilder.Services.AddSerilog(config => {
            config.AsAnnaSasDevServerConsole(24);
        });
        
        webAppBuilder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
        
        webAppBuilder.WebHost.UseStaticWebAssets();
        
        webAppBuilder.Services.AddInfiniFrameJs();

        InfiniFrameWindowBuilder windowBuilder = applicationBuilder.Window;
        windowBuilder.Center()
            .SetUseOsDefaultSize(true)
            .RegisterOpenExternalTargetWebMessageHandler()
            .SetTitle("InfiniLore Sample");
        
        // -------------------------------------------------------------------------------------------------------------
        // Application
        // -------------------------------------------------------------------------------------------------------------
        InfiniFrameWebApplication application = applicationBuilder.Build();
        WebApplication webApp = application.WebApp;

        webApp.UseFastEndpoints()
            .UseSwaggerGen();

        webApp.UseHttpsRedirection();
        
        webApp.UseStaticFiles();

        webApp.UseAntiforgery();

        webApp.MapStaticAssets();
        webApp.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
            .AddAdditionalAssemblies(typeof(Routes).Assembly);

        webApp.UseInfiniLoreApplication();
        
        application.Run();
    }
}
