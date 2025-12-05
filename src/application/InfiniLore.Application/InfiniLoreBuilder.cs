// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.CliArgsParser;
using FastEndpoints;
using FastEndpoints.Swagger;
using InfiniLore.Core.Database;
using InfiniLore.Core.Modular;
using InfiniLore.Modules.Assets;
using InfiniLore.Modules.Projects;
using InfiniLore.Modules.Users;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace InfiniLore.Application;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class InfiniLoreBuilder {
    public static IServiceCollection AddInfiniLoreApplication(this IServiceCollection services) {
        services.AddInfiniModuleProvider(
            collection => {
                collection.AddModule<UsersInfiniModule>();
                collection.AddModule<ProjectsInfiniModule>();
                collection.AddModule<AssetsInfiniModule>();
            },
            out InfiniModuleCollection moduleCollection
        );

        services.AddInfiniLoreDb(
            options => {
                const string dbFile = "InfiniLore.db";
                var connection = new SqliteConnection($"DataSource={dbFile}");
                connection.Open();

                options.UseSqlite(connection);
            },
            moduleCollection.Assemblies
        );

        services.AddFastEndpoints(options => options.Assemblies = moduleCollection.Assemblies);
        services.SwaggerDocument();

        services.AddInfiniBlazor(config => {
            config.Components.SetRenderMode(RenderMode.InteractiveServer);
        });

        services.AddSingleton<ICliParser>(provider => {
            ICliParserBuilder cliBuilder = CliParser.CreateBuilder()
                .WithServiceProvider(provider);

            foreach (Assembly assembly in moduleCollection.Assemblies) {
                cliBuilder.AddFromAssembly(assembly);
            }

            return cliBuilder.Build();
        });

        // Onboarding service
        services.AddScoped<UserOnboarding>();

        return services;
    }

    public static WebApplication UseInfiniLoreApplication(this WebApplication app) {
        EnsureDatabaseCreated(app);

        app.UseInfiniLoreModules();

        return app;
    }

    private static void EnsureDatabaseCreated(WebApplication app) {
        using IServiceScope scope = app.Services.CreateScope();
        using var db = scope.ServiceProvider.GetRequiredService<InfiniLoreDb>();
        db.Database.EnsureCreated();
    }

    public static ICliParserBuilder GetInfiniLoreCliParserBuilder(this WebApplication app) {
        var moduleCollection = app.Services.GetService<InfiniModuleProvider>();

        ICliParserBuilder cliBuilder = CliParser.CreateBuilder()
            .WithServiceProvider(app.Services);

        IEnumerable<Assembly> assemblies = moduleCollection?.GetRegisteredAssemblies() ?? Enumerable.Empty<Assembly>();
        foreach (Assembly assembly in assemblies) {
            cliBuilder.AddFromAssembly(assembly);
        }

        return cliBuilder;
    }
}
