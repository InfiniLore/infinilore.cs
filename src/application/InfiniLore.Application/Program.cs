// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using FastEndpoints.Swagger;
using InfiniFrame;
using InfiniFrame.Js;
using InfiniFrame.Js.MessageHandlers;
using InfiniFrame.WebServer;
using InfiniLore.Application.Components;
using InfiniLore.Core.Modular;
using Serilog;

namespace InfiniLore.Application;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class Program {
    public static InfiniFrameWebApplication Application { get; private set; } = null!;
    
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
        
        webAppBuilder.Services.AddInfiniFrameJs();

        webAppBuilder.Services.RegisterServicesFromInfiniLoreApplication();
        
        webAppBuilder.WebHost.UseStaticWebAssets();

        InfiniFrameWindowBuilder windowBuilder = applicationBuilder.Window;
        windowBuilder.Center()
            .SetUseOsDefaultSize(true)
            .RegisterFullScreenWebMessageHandler()
            .RegisterOpenExternalTargetWebMessageHandler()
            .RegisterTitleChangedWebMessageHandler()
            .RegisterWindowManagementWebMessageHandler()
            .SetTitle("InfiniLore");
        
        // -------------------------------------------------------------------------------------------------------------
        // Application
        // -------------------------------------------------------------------------------------------------------------
        InfiniFrameWebApplication application = applicationBuilder.Build();
        application.UseAutoServerClose();

        Application = application;

        WebApplication webApp = application.WebApp;

        webApp.UseHttpsRedirection();

        webApp.UseRouting();

        webApp.UseAntiforgery();

        webApp.UseFastEndpoints()
            .UseSwaggerGen();

        webApp.MapStaticAssets();
        webApp.UseStaticFiles();

        webApp.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
            .AddInfiniLoreModuleAssemblies(webApp);
        
        // webApp.UseInfiniLoreApplication();

        if (args.Length != 0) {
            var cli = new InfiniLoreCli(webApp);
            cli.Run(args);
        }
        
        application.Run();
    }
    
}
