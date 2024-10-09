// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AspNetCore.Swagger.Themes;
using CodeOfChaos.Extensions.AspNetCore;
using FastEndpoints;
using FastEndpoints.Swagger;
using InfiniLore.Server.API;
using InfiniLore.Server.Components;
using InfiniLore.Server.Data;
using InfiniLore.Server.Data.Models.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InfiniLore.Server;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class Program {
    public static void Main(string[] args) {
        // -------------------------------------------------------------------------------------------------------------
        // Builder
        // -------------------------------------------------------------------------------------------------------------
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        builder.OverrideLoggingAsSeriLog();

        // TODO: Add Kestrel SLL
        //  This will take some tweaking to get working
         
        #region Databasee
        builder.Services.AddDbContextFactory<InfiniLoreDbContext>();
        builder.Services.AddIdentity<InfiniLoreUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<InfiniLoreDbContext>();
        #endregion
        #region Razor Components
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents();
        #endregion
        #region FastEndpoints & Swagger
        builder.Services
            .AddFastEndpoints(options => {
                options.Assemblies = [
                    typeof(API.IAssemblyEntry).Assembly
                ];
            })
            .SwaggerDocument(options => {
                options.DocumentSettings = settings => {
                    settings.Version = "v1";
                    settings.Title = "InfiniLore API v1";
                    settings.Description = "An ASP.NET Core Web API for managing InfiniLore";
                };
            })
        ;
        #endregion

        builder.Services.RegisterServicesFromInfiniLoreServerData();
        builder.Services.RegisterServicesFromInfiniLoreServerAPI();
        
        // TODO Add google oauth login

        // -------------------------------------------------------------------------------------------------------------
        // App
        // -------------------------------------------------------------------------------------------------------------
        WebApplication app = builder.Build();

        if (app.Environment.IsDevelopment()) {
            app.UseWebAssemblyDebugging();
        }
        else {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
            .AddInteractiveWebAssemblyRenderMode()
            .AddAdditionalAssemblies(typeof(WasmClient.IAssemblyEntry).Assembly);

        app.UseFastEndpoints();
        app.UseOpenApi();
        app.UseSwaggerUI(ModernStyle.Dark, ctx => {
            ctx.SwaggerEndpoint("/swagger/v1/swagger.json", "InfiniLore API v1");
            ctx.RoutePrefix = "swagger";
        });
        
        // TODO Check if applying the migrations is actually correct here
        using InfiniLoreDbContext db = app.Services.GetRequiredService<IDbContextFactory<InfiniLoreDbContext>>().CreateDbContext();
        db.Database.Migrate();
        db.SaveChanges();
        
        app.Run();
    }
}
