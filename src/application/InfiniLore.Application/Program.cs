// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.CliArgsParser;
using FastEndpoints;
using FastEndpoints.Swagger;
using InfiniFrame;
using InfiniFrame.Js;
using InfiniFrame.Js.MessageHandlers;
using InfiniFrame.WebServer;
using InfiniLore.Application.Components;
using InfiniLore.Core;
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
        
        webAppBuilder.WebHost.UseStaticWebAssets();
        
        webAppBuilder.Services.AddInfiniFrameJs();

        webAppBuilder.Services.RegisterServicesFromInfiniLoreApplication();

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
        Application = application;
        
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

        if (args.Length != 0) {
            ICliParserBuilder cliParserBuilder = webApp.GetInfiniLoreCliParserBuilder();
            cliParserBuilder.AddFromAssembly(typeof(Program).Assembly);
            ICliParser cliParser = cliParserBuilder.Build();
            try {
                Task.Run(async () => await cliParser.ExecuteAsync(args)).Wait();
            }
            catch (Exception e) {
                Log.Error(e, "Failed to parse command line arguments.");
                Environment.Exit(-1);
            }
        }
        
        application.Run();
    }
}
