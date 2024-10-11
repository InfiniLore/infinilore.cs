// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AspNetCore.Swagger.Themes;
using CodeOfChaos.Extensions.AspNetCore;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using InfiniLore.Server.API;
using InfiniLore.Server.Components;
using InfiniLore.Server.Contracts.Repositories;
using InfiniLore.Server.Data;
using InfiniLore.Server.Data.Models.Account;
using InfiniLore.Server.Data.Repositories.UserData;
using InfiniLore.Server.Services;
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

        #region Database
        builder.Services.AddDbContextFactory<InfiniLoreDbContext>();
        #endregion

        #region Authentication
        // Register JWT Authentication
        builder.Services.AddAuthenticationJwtBearer(options => {
            options.SigningKey = builder.Configuration["JWT:Key"];
        });
        
        // TODO Add google oauth login

        // Register Identity
        builder.Services.AddIdentity<InfiniLoreUser, IdentityRole>(options => 
                options.SignIn.RequireConfirmedAccount = true
            )
            .AddEntityFrameworkStores<InfiniLoreDbContext>()
            .AddSignInManager();

        // Override cookie auth scheme to return 401/403 instead of redirecting
        builder.Services.ConfigureApplicationCookie(
            c => {
                c.Events.OnRedirectToLogin = ctx => {
                    if (ctx is { Request.Path.Value: "/api", Response.StatusCode: 200 }) {
                        ctx.Response.StatusCode = 401;
                    }
                    return Task.CompletedTask;
                };

                c.Events.OnRedirectToAccessDenied = ctx => {
                    if (ctx is { Request.Path.Value: "/api", Response.StatusCode: 200 }) {
                        ctx.Response.StatusCode = 403;
                    }
                    return Task.CompletedTask;
                };
            });
        #endregion

        #region Authorization
        builder.Services.AddAuthorization();
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
            });
        #endregion

        builder.Services.RegisterServicesFromInfiniLoreServerAPI();
        builder.Services.AddIdentityApiEndpoints<InfiniLoreUser>();

        builder.Services.RegisterServicesFromInfiniLoreServerData();
        builder.Services.AddScoped(typeof(IAuditLogRepository<>), typeof(AuditLogRepository<>));
        builder.Services.RegisterServicesFromInfiniLoreServerServices();

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

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
            .AddInteractiveWebAssemblyRenderMode()
            .AddAdditionalAssemblies(typeof(WasmClient.IAssemblyEntry).Assembly);

        app.UseFastEndpoints(ctx => {
            ctx.Endpoints.RoutePrefix = "api";
        });
        app.UseOpenApi();
        app.UseSwaggerUI(ModernStyle.Dark, setupAction: ctx => {
            ctx.SwaggerEndpoint("/swagger/v1/swagger.json", "InfiniLore API v1");
            ctx.RoutePrefix = "swagger";
        });

        // TODO Check if applying the migrations is actually correct here
        using (InfiniLoreDbContext db = app.Services.GetRequiredService<IDbContextFactory<InfiniLoreDbContext>>().CreateDbContext()) {
            db.Database.Migrate();
            db.SaveChanges();
        }

        app.Run();
    }
}