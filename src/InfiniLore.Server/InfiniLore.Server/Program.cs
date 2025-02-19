// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Auth0.AspNetCore.Authentication;
using CodeOfChaos.Extensions.AspNetCore;
using FastEndpoints;
using FastEndpoints.Swagger;
using InfiniLore.Clients.Wasm;
using InfiniLore.Server.Api;
using InfiniLore.Server.Components;
using InfiniLore.Server.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Core;
using System.Security.Claims;

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
        LoggingLevelSwitch loggingLevelSwitch = new();
        LoggerConfiguration loggerConfig = new LoggerConfiguration()
            .MinimumLevel.ControlledBy(loggingLevelSwitch);

        loggerConfig.AsAnnaSasDevServerConsole();

        Log.Logger = loggerConfig.CreateLogger();


        // Clear default providers and setup Serilog
        builder.Logging.ClearProviders();
        // builder.Logging.AddSerilog(Log.Logger);
        // builder.Services.AddSingleton(Log.Logger);

        builder.Services.AddHostedService<LoggingOverrideExtensions.ApplicationShutdownLoggerCleanup>(); // Ensure cleanup
        builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(Log.Logger));
        
        // builder.OverrideLoggingWithSerilog(config => config.AsAnnaSasDevServerConsole());

        #region Database
        // Technically we need to wrap this as a `IsDevelopment`
        //      And have another value for when we don't pull from our own container 
        string connectionString = await ContentDbFactory.CreateDockerMsSqlContainer();

        // Most of the DB registration is handled through the Factory class
        //      Some extra setup is required on this end though
        //      We need to register what db we are using, this way we can reuse the factory for testing, etc...
        ContentDbFactory.RegisterDatabase(builder.Services, optionsAction: options => {
            options.UseSqlServer(connectionString);
        });
        #endregion

        builder.Services.AddMemoryCache();

        #region Auth0
        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => {
                options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}/";
                ;
                options.Audience = builder.Configuration["Auth0:Audience"];
                options.TokenValidationParameters = new TokenValidationParameters {
                    NameClaimType = ClaimTypes.NameIdentifier
                };
            });
        builder.Services.AddAuthorizationCore();
        builder.Services.AddCascadingAuthenticationState();
        

        builder.Services.AddAuth0WebAppAuthentication(options => {
            options.Domain = builder.Configuration["Auth0:Domain"]!;
            options.ClientId = builder.Configuration["Auth0:ClientId"]!;
            options.Scope = "openid profile email";
            options.CallbackPath = new PathString("/auth/callback");
        });
        builder.Services.AddScoped<TokenProvider>();
        builder.Services.AddScoped<InitialApplicationState>();
        #endregion

        #region FastEndpoints
        builder.Services.AddFastEndpoints(options => {
            options.Assemblies = [
                typeof(IEntrypointInfiniLoreServerApi).Assembly
            ];
        });
        builder.Services.SwaggerDocument();
        #endregion

        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents()
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
            app.UseHsts();// Default is 30 days, change to something else for production
        }

        app.UseHttpsRedirection();

        app.UseAntiforgery();

        app.MapStaticAssets();


        app.UseAuthentication();
        app.UseAuthorization();
        app.UseFastEndpoints(config => {
            config.Endpoints.RoutePrefix = "api/v1";
        });
        app.UseSwaggerGen();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
            .AddInteractiveWebAssemblyRenderMode()
            .AddAdditionalAssemblies(typeof(IEntrypointInfiniLoreClientsWasm).Assembly);

        await using (ContentDb db = await app.Services.GetRequiredService<IDbContextFactory<ContentDb>>().CreateDbContextAsync()) {
            await db.Database.MigrateAsync();
            await db.SaveChangesAsync();
        }

        await app.RunAsync();
    }
}
