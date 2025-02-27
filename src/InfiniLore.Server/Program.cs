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
using InfiniLore.Server.DataSeeder;
using InfiniLore.Server.Services;
using InfiniLore.Server.Services.CQRS;
using InfiniLore.Server.Services.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Security.Claims;
using TokenValidatedContext=Microsoft.AspNetCore.Authentication.OpenIdConnect.TokenValidatedContext;

namespace InfiniLore.Server;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class Program {
    public static async Task Main(string[] args) {
        // Builder is set up here first
        //      This is so we can override the logging configuration
        //      And have proper application exception catching 
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        builder.OverrideLoggingWithSerilog(config => config
            .AsAnnaSasDevServerConsole(sectionMaxLength: 24)
            .WithTruncateSourceContextEnricher(maxLength: 24)
        );

        try {
            WebApplication app = await BuildApp(builder);
            await Start(app);
        }
        catch (Exception ex) {
            Log.Logger.Fatal(ex, "Host terminated unexpectedly: {Message} \n {Trace}", ex.Message, ex.StackTrace);
        }
        finally {
            await Log.CloseAndFlushAsync();
        }
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Builder
    // -----------------------------------------------------------------------------------------------------------------
    private static async Task<WebApplication> BuildApp(WebApplicationBuilder builder) {
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

        #region Auth
        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, configureOptions: options => {
                options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}/";
                options.Audience = builder.Configuration["Auth0:Audience"];
                options.TokenValidationParameters = new TokenValidationParameters {
                    NameClaimType = ClaimTypes.NameIdentifier
                };
            });

        builder.Services.AddOptions();
        builder.Services.AddAuth0WebAppAuthentication(options => {
            ArgumentNullException.ThrowIfNull(builder.Configuration["Auth0:Domain"]);
            options.Domain = builder.Configuration["Auth0:Domain"]!;

            ArgumentNullException.ThrowIfNull(builder.Configuration["Auth0:ClientId"]);
            options.ClientId = builder.Configuration["Auth0:ClientId"]!;

            // Add ClientSecret
            // options.ClientSecret = builder.Configuration["Auth0:ClientSecret"]!;
            options.Scope = "openid profile email";
            options.CallbackPath = "/auth/callback";

            options.OpenIdConnectEvents = new OpenIdConnectEvents {
                OnTokenValidated = OpenIdConnectEventHelper.HandleWith<TokenValidatedContext>()
            };
        });

        builder.Services.AddAuthorization();
        builder.Services.AddCascadingAuthenticationState();
        #endregion

        #region FastEndpoints
        builder.Services.AddFastEndpoints(options => {
            options.DisableAutoDiscovery = true;

            options.Assemblies = [
                typeof(IEntrypointInfiniLoreServerApi).Assembly
            ];
        });
        builder.Services.SwaggerDocument();
        #endregion

        #region MediatR
        builder.Services.AddMediatR(config => {
            config.RegisterServicesFromAssembly(typeof(IEntrypointInfiniLoreServerServicesCqrs).Assembly);
        });
        #endregion

        builder.Services.AddHttpClient();
        builder.Services.AddMemoryCache();
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents();

        builder.Services.RegisterServicesFromInfiniLoreServerServices();

        #region DataSeeding
        // Everything is handled by the DataSeeding project
        //      This is to make sure we don't have any issues with the seeding process
        //      And to make sure we've enabled overloading of the method
        //      We also migrate the db in this step, if required.
        //          (Which could be a problem long term, if we have a lot of migrations that drop data, but those are future Anna's problems)
        builder.RegisterDataSeedingServices(); 
        #endregion

        return builder.Build();
    }

    // -----------------------------------------------------------------------------------------------------------------
    // App
    // -----------------------------------------------------------------------------------------------------------------
    private static async Task Start(WebApplication app) {
        if (app.Environment.IsDevelopment()) {
            app.UseWebAssemblyDebugging();
        }
        else {
            app.UseExceptionHandler("/Error");
            app.UseHsts();// Default is 30 days, change to something else for production
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.UseAuthentication();
        app.UseAuthorization();

        #region Authentication Endpoints
        app.MapGet("/account/login", handler: async Task (HttpContext httpContext, string redirectUri = "/") => {
            AuthenticationProperties authenticationProperties = new LoginAuthenticationPropertiesBuilder()
                .WithRedirectUri(redirectUri)
                .Build();

            await httpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        });

        app.MapGet("/account/logout", handler: async Task (HttpContext httpContext, string redirectUri = "/") => {
            AuthenticationProperties authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
                .WithRedirectUri(redirectUri)
                .Build();

            await httpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        });
        #endregion

        app.UseFastEndpoints(config => {
            config.Endpoints.RoutePrefix = "api/v1";
            config.Errors.UseProblemDetails();
        });
        app.UseSwaggerGen();

        app.MapStaticAssets();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
            .AddInteractiveWebAssemblyRenderMode()
            .AddAdditionalAssemblies(typeof(IEntrypointInfiniLoreClientsWasm).Assembly);
        
        await app.RunAsync();
    }
}
