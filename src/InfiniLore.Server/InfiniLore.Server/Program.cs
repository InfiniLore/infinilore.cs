// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.AspNetCore;
using InfiniLore.Clients.Wasm;
using InfiniLore.Server.Components;
using InfiniLore.Server.Database;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace InfiniLore.Server;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class Program {
    public static async Task Main(string[] args) {
        // -------------------------------------------------------------------------------------------------------------
        // Builder
        // -------------------------------------------------------------------------------------------------------------
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        builder.OverrideLoggingWithSerilog(config => config.AsAnnaSasDevServerConsole());

        #region Database
        // Technically we need to wrap this as a `IsDevelopment`
        //      And have another value for when we don't pull from our own container 
        string connectionString = await ContentDbFactory.CreateDockerMsSqlContainer();
        
        // Most of the DB registration is handled through the Factory class
        //      Some extra setup is required on this end though
        //      We need to register what db we are using, this way we can reuse the factory for testing, etc...
        ContentDbFactory.RegisterDatabase(builder.Services, options => {
            options.UseSqlServer(connectionString);
        });
        #endregion
        
        builder.Services.AddRazorComponents()
            .AddInteractiveWebAssemblyComponents();
        
        // -------------------------------------------------------------------------------------------------------------
        // App
        // -------------------------------------------------------------------------------------------------------------
        WebApplication app = builder.Build();

        if (app.Environment.IsDevelopment()) {
            app.UseWebAssemblyDebugging();
        }
        else {
            app.UseExceptionHandler("/Error");
            app.UseHsts(); // Default is 30 days, change to something else for production
        }

        app.UseHttpsRedirection();

        app.UseAntiforgery();

        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveWebAssemblyRenderMode()
            .AddAdditionalAssemblies(typeof(IEntrypointInfiniLoreClientsWasm).Assembly);

        await using (ContentDb db = await app.Services.GetRequiredService<IDbContextFactory<ContentDb>>().CreateDbContextAsync()) {
            await db.Database.MigrateAsync();
            await db.SaveChangesAsync();
        }

        await app.RunAsync();
    }
}
