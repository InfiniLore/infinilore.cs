// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using FastEndpoints.Swagger;
using InfiniFrame;
using InfiniFrame.Server;
using InfiniLore.Core;
using InfiniLore.Core.Modular;
using InfiniLore.Modules.Assets;
using InfiniLore.Modules.Projects;
using InfiniLore.Modules.Users;
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

        appBuilder.Services.AddInfiniModuleProvider(moduleCollection => {
                moduleCollection.AddModule<UsersInfiniModule>();
                moduleCollection.AddModule<ProjectsInfiniModule>();
                moduleCollection.AddModule<AssetsInfiniModule>();
            },
            out InfiniModuleProvider moduleProvider
        );
        
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
        
        appBuilder.Services.AddFastEndpoints(options => options.Assemblies = moduleProvider.Assemblies);
        
        appBuilder.Services.SwaggerDocument();
        
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
        
        infiniFrameServer.MapInfiniFrameJsEndpoints();
        
        // infiniFrameServer.WebApp.MapGet("/_content/InfiniLore.InfiniBlazor/InfiniBlazor.js", requestDelegate: async context => {
        //     Assembly assembly = typeof(InfiniBlazorConfig).Assembly;
        //     const string resourceName = "InfiniLore.InfiniBlazor.wwwroot.InfiniBlazor.js";
        //
        //     await using Stream? stream = assembly.GetManifestResourceStream(resourceName);
        //     if (stream == null) {
        //         context.Response.StatusCode = 404;
        //         await context.Response.WriteAsync("Resource not found");
        //         return;
        //     }
        //
        //     context.Response.ContentType = "application/javascript";
        //     await stream.CopyToAsync(context.Response.Body);
        // });
        //
        // infiniFrameServer.WebApp.MapGet("/_content/InfiniLore.InfiniBlazor/InfiniBlazor.css", requestDelegate: async context => {
        //     Assembly assembly = typeof(InfiniBlazorConfig).Assembly;
        //     const string resourceName = "InfiniLore.InfiniBlazor.wwwroot.InfiniBlazor.css";
        //
        //     await using Stream? stream = assembly.GetManifestResourceStream(resourceName);
        //     if (stream == null) {
        //         context.Response.StatusCode = 404;
        //         await context.Response.WriteAsync("Resource not found");
        //         return;
        //     }
        //
        //     context.Response.ContentType = "text/css";
        //     await stream.CopyToAsync(context.Response.Body);
        // });
        
        infiniFrameServer.Run();

        IInfiniFrameWindowBuilder windowBuilder = infiniFrameServer.GetAttachedWindowBuilder()
            .Center()
            .SetUseOsDefaultSize(true)
            .SetTitle("InfiniLore Sample");

        IInfiniFrameWindow window = windowBuilder.Build();

        window.WaitForClose();
        
    }
}
