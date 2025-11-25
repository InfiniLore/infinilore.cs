// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using FastEndpoints.Swagger;
using InfiniFrame;
using InfiniFrame.Js.MessageHandlers;
using InfiniFrame.Server;
using InfiniLore.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Web;
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
        var infiniFrameServerBuilder = InfiniFrameServerBuilder.Create("wwwroot", args);
        WebApplicationBuilder appBuilder = infiniFrameServerBuilder.WebAppBuilder;

        appBuilder.Services.AddInfiniLoreApplication();
        
        appBuilder.Services.AddLogging(config => {
            config.ClearProviders();
            config.AddSerilog();
        });
        
        appBuilder.Services.AddSerilog(config => {
            config.AsAnnaSasDevServerConsole(24).MinimumLevel.Debug();
        });
        
        appBuilder.Services.AddInfiniBlazor(config => {
            config.Components.SetRenderMode(RenderMode.InteractiveServer);
        });
        
        appBuilder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
        
        appBuilder.WebHost.UseStaticWebAssets();
        
        // -------------------------------------------------------------------------------------------------------------
        // Application
        // -------------------------------------------------------------------------------------------------------------
        InfiniFrameServer infiniFrameServer = infiniFrameServerBuilder.Build();
        WebApplication app = infiniFrameServer.WebApp;

        app.UseFastEndpoints()
            .UseSwaggerGen();
        
        app.UseHttpsRedirection();

        app.UseAntiforgery();

        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();
        
        app.UseInfiniLoreApplication();
        
        infiniFrameServer.MapInfiniFrameJsEndpoints();
        
        infiniFrameServer.Run();

        IInfiniFrameWindowBuilder windowBuilder = infiniFrameServer.GetAttachedWindowBuilder()
            .Center()
            .SetUseOsDefaultSize(true)
            .RegisterOpenExternalTargetWebMessageHandler()
            .SetTitle("InfiniLore Sample");

        IInfiniFrameWindow window = windowBuilder.Build();

        window.WaitForClose();
        
    }
}
