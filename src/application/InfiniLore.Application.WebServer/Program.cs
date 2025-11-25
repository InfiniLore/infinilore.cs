// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using FastEndpoints.Swagger;
using InfiniLore.Application.WebServer.Components;
using InfiniLore.Core;
using Serilog;

namespace InfiniLore.Application.WebServer;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class Program {
    public static void Main(string[] args) {
        // -------------------------------------------------------------------------------------------------------------
        // Builder
        // -------------------------------------------------------------------------------------------------------------
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddInfiniLoreApplication();

        builder.Services.AddLogging(config => {
            config.ClearProviders();
            config.AddSerilog();
        });

        builder.Services.AddSerilog(config => {
            config.AsAnnaSasDevServerConsole(24).MinimumLevel.Debug();
        });

        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
    
        builder.WebHost.UseStaticWebAssets();

        // -------------------------------------------------------------------------------------------------------------
        // Application
        // -------------------------------------------------------------------------------------------------------------
        WebApplication app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment()) {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseFastEndpoints()
            .UseSwaggerGen();

        app.UseHttpsRedirection();
        
        app.UseStaticFiles();

        app.UseAntiforgery();

        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
            .AddAdditionalAssemblies(typeof(Routes).Assembly);

        app.UseInfiniLoreApplication();

        app.Run();
    }
}