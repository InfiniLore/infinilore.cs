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
using Microsoft.AspNetCore.Authentication.Cookies;
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
        
        webAppBuilder.Services.AddHttpContextAccessor();
        
        webAppBuilder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options => {
                options.LoginPath = "/signup";
                options.ExpireTimeSpan = TimeSpan.FromDays(30);
                options.SlidingExpiration = true;
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Strict;
            });
        
        webAppBuilder.Services.AddAuthorization();

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
        
        webApp.UseAuthentication();
        webApp.UseAuthorization();

        webApp.UseAntiforgery();

        webApp.UseFastEndpoints()
            .UseSwaggerGen();

        webApp.MapStaticAssets();
        webApp.UseStaticFiles();

        webApp.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
            .AddInfiniLoreModuleAssemblies(webApp);
        
        webApp.UseInfiniLoreApplication();

        if (args.Length != 0) {
            var cli = new InfiniLoreCli(webApp);
            cli.Run(args);
        }
        
        application.Run();
    }
    
}
