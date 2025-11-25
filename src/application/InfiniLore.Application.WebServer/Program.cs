// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using FastEndpoints.Swagger;
using InfiniLore.Core;
using InfiniLore.Core.Database;
using InfiniLore.Core.Modular;
using InfiniLore.Modules.Assets;
using InfiniLore.Modules.Projects;
using InfiniLore.Modules.Users;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
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

        builder.Services.AddInfiniModuleProvider(moduleCollection => {
                                                        moduleCollection.AddModule<UsersInfiniModule>();
                                                        moduleCollection.AddModule<ProjectsInfiniModule>();
                                                        moduleCollection.AddModule<AssetsInfiniModule>();
                                                    },
                                                    out InfiniModuleProvider moduleProvider
        );
        
        builder.Services.AddInfiniLoreDb(
            options => {
                const string dbFile = "InfiniLore.db";
                var connection = new SqliteConnection($"DataSource={dbFile}");
                connection.Open();

                options.UseSqlite(connection);
            },
            moduleProvider.Assemblies
        );
        
        builder.Services.AddLogging(config => {
            config.ClearProviders();
            config.AddSerilog();
        });
        
        builder.Services.AddSerilog(config => {
            config.AsAnnaSasDevServerConsole(24).MinimumLevel.Debug();
        });
        
        builder.Services.AddInfiniBlazor(config => {
            config.Components.SetRenderMode(RenderMode.InteractiveServer);
        });
        
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
        
        builder.Services.AddFastEndpoints(
            options => options.Assemblies = moduleProvider.Assemblies);
        
        builder.Services.SwaggerDocument();

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

        app.UseAntiforgery();

        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}
