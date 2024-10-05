// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AspNetCore.Swagger.Themes;
using CodeOfChaos.Extensions.AspNetCore;
using FastEndpoints;
using InfiniLore.Server.Components;
using InfiniLore.Server.Database;
using InfiniLore.Server.Database.Models.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

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
         
        // Db stuff
        builder.Services.AddDbContextFactory<InfiniLoreDbContext>();
        builder.Services.AddIdentity<InfiniLoreUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<InfiniLoreDbContext>();
            
        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents();
        
        builder.Services.AddFastEndpoints(options => {
            options.Assemblies = [typeof(API.IAssemblyEntry).Assembly]; // Add InfiniLore API
        });
        
        // - Swagger -
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options => {
            options.SwaggerDoc("v1", new OpenApiInfo {
                Version = "v1",
                Title = "InfiniLore API v1",
                Description = "An ASP.NET Core Web API for managing InfiniLore",
            });
            options.EnableAnnotations();
        });
        
        // TODO Add API Endpoints
        
        // TODO Add google oauth login
        
        // TODO Create rules to for where stuff has to go

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
            // TODO add an IAssemblyEntry into the Client and use here
            .AddAdditionalAssemblies(typeof(Client._Imports).Assembly); 
        
        app.UseSwagger();
        app.UseSwaggerUI(ModernStyle.Dark, ctx => {
            ctx.SwaggerEndpoint("/swagger/v1/swagger.json", "InfiniLore API v1");
            ctx.RoutePrefix = "swagger";
        });

        app.UseFastEndpoints();

        // TODO Check if applying the migrations is actually correct here
        using InfiniLoreDbContext db = app.Services.GetRequiredService<IDbContextFactory<InfiniLoreDbContext>>().CreateDbContext();
        db.Database.Migrate();
        db.SaveChanges();
        
        app.Run();
    }
}
