// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using FastEndpoints.Swagger;
using InfiniLore.Core.Database;
using InfiniLore.Core.Modular;
using InfiniLore.Modules.Assets;
using InfiniLore.Modules.Projects;
using InfiniLore.Modules.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Application;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class InfiniLoreBuilder {
    public static IServiceCollection AddInfiniLoreApplication(this IServiceCollection services) {
        services.AddInfiniModuleProvider(
            moduleCollection => {
                moduleCollection.AddModule<UsersInfiniModule>();
                moduleCollection.AddModule<ProjectsInfiniModule>();
                moduleCollection.AddModule<AssetsInfiniModule>();
            },
            out InfiniModuleProvider moduleProvider
        );

        services.AddInfiniLoreDb(
            options => {
                const string dbFile = "InfiniLore.db";
                var connection = new SqliteConnection($"DataSource={dbFile}");
                connection.Open();

                options.UseSqlite(connection);
            },
            moduleProvider.Assemblies
        );
        
        services.AddFastEndpoints(options => options.Assemblies = moduleProvider.Assemblies);
        services.SwaggerDocument();

        services.AddInfiniBlazor(config => {
            config.Components.SetRenderMode(RenderMode.InteractiveServer);
        });
        
        return services;
    }

    public static WebApplication UseInfiniLoreApplication(this WebApplication app) {
        
        EnsureDatabaseCreated(app);

        return app;
    }
    
    private static void EnsureDatabaseCreated(WebApplication app) {
        using IServiceScope scope = app.Services.CreateScope();
        using var db = scope.ServiceProvider.GetRequiredService<InfiniLoreDb>();
        db.Database.EnsureCreated();
    }
}
