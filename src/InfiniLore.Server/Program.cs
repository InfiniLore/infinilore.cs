// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Auth0.AspNetCore.Authentication;
using CodeOfChaos.Extensions.AspNetCore;
using FastEndpoints;
using FastEndpoints.Swagger;
using InfiniLore.Blazor.Markdown;
using InfiniLore.Clients.Wasm;
using InfiniLore.Credentials.Auth0.DependencyInjection;
using InfiniLore.Server.Api;
using InfiniLore.Server.Api.Responses;
using InfiniLore.Server.Components;
using InfiniLore.Server.Database;
using InfiniLore.Server.DataSeeder;
using InfiniLore.Server.Services;
using InfiniLore.Server.Services.Auth0.Encryption;
using InfiniLore.Server.Services.Auth0.TokenStore;
using InfiniLore.Server.Services.Mediator;
using InfiniLore.Server.Services.OpenIdConnect;
using InfiniLore.ServerClient.Shared;
using InfiniLore.ServerClient.Shared.JwtToken;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Security.Claims;
using Wolverine;
using Wolverine.FluentValidation;

namespace InfiniLore.Server;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class Program {
    public static async Task Main(string[] args) {
        await GlobalExceptionHandler.ExecuteWithGlobalExceptionHandlingAsync(async () => {
            // Builder is set up here first
            //      This is so we can override the logging configuration
            //      And have proper application exception catching 
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            builder.OverrideLoggingWithSerilog(config => config
                .AsAnnaSasDevServerConsole(
                    24,
                    configure: asyncConsoleConfig => asyncConsoleConfig.ApplyThemeToRedirectedOutput = true// Needed for nice DotnetWatch console output    
                )
                .WithTruncateSourceContextEnricher(maxLength: 24)
            );

            WebApplication app = await BuildApp(builder);
            await Start(app);
        });
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
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    NameClaimType = ClaimTypes.NameIdentifier
                };
            });

        builder.Services.AddOptions();
        builder.Services.AddAuth0WebAppAuthentication(options => {
            ArgumentNullException.ThrowIfNull(builder.Configuration["Auth0:Domain"]);
            options.Domain = builder.Configuration["Auth0:Domain"]!;

            ArgumentNullException.ThrowIfNull(builder.Configuration["Auth0:ClientId-WebApp"]);
            options.ClientId = builder.Configuration["Auth0:ClientId-WebApp"]!;

            ArgumentNullException.ThrowIfNull(builder.Configuration["Auth0:ClientSecret-WebApp"]);
            options.ClientSecret = builder.Configuration["Auth0:ClientSecret-WebApp"]!;

            options.Scope = "openid profile email";
            options.CallbackPath = "/auth/callback";

            options.OpenIdConnectEvents = new OpenIdConnectEvents {
                OnTokenValidated = OpenIdConnectEventHelper.OnTokenValidated
            };
        }).WithAccessToken(options => {
            ArgumentNullException.ThrowIfNull(builder.Configuration["Auth0:Audience"]);
            options.Audience = builder.Configuration["Auth0:Audience"]!;
            options.UseRefreshTokens = true;
        });

        builder.Services.AddAuthorizationBuilder()
            .AddJwtProtectedPolicy();

        builder.Services.AddCascadingAuthenticationState();
        #endregion

        #region Auth0 Management Services
        builder.AddAuth0ManagementApiServices(config => {
            config.SetScopedTokenStore<MediatorProxyAccessTokenStore>();

            ArgumentNullException.ThrowIfNull(builder.Configuration["Auth0:Domain"]);
            config.Auth0Options.Domain = builder.Configuration["Auth0:Domain"]!;

            ArgumentNullException.ThrowIfNull(builder.Configuration["Auth0:ClientId-Management"]);
            config.Auth0Options.ClientId = builder.Configuration["Auth0:ClientId-Management"]!;

            ArgumentNullException.ThrowIfNull(builder.Configuration["Auth0:ClientSecret-Management"]);
            config.Auth0Options.ClientSecret = builder.Configuration["Auth0:ClientSecret-Management"]!;
        });

        builder.AddAuth0AccessTokenEncryptionOptions();// Required to set options correctly
        #endregion
        
        #region Wolverine
        builder.Host.UseWolverine(options => {
            options.UseFluentValidation();
            options.Discovery.IncludeAssembly(typeof(IEntrypointInfiniLoreServerServicesCqrs).Assembly);
        });
        #endregion

        #region FastEndpoints
        builder.Services.AddFastEndpoints(options => {
            options.DisableAutoDiscovery = true;

            options.Assemblies = [
                typeof(IEntrypointInfiniLoreServerApi).Assembly,
                typeof(IEntrypointInfiniLoreServerApiResponses).Assembly,
                typeof(IEntrypointInfiniLoreServerServicesCqrs).Assembly
            ];
        });

        builder.Services.SwaggerDocument();
        builder.Services.RegisterServicesFromInfiniLoreServerServicesMediator();
        #endregion
        
        #region DataSeeding
        // Everything is handled by the DataSeeding project
        //      This is to make sure we don't have any issues with the seeding process
        //      And to make sure we've enabled overloading of the method
        //      We also migrate the db in this step, if required.
        //          (Which could be a problem long term, if we have a lot of migrations that drop data, but those are future Anna's problems)
        builder.RegisterDataSeedingServices();
        #endregion
        
        #region InfiniLore.Blazor
        builder.Services.AddInfiniLoreBlazor(config => {
            config.AddMarkdown();
        });
        #endregion

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddHttpClient();
        builder.Services.AddHttpClient("ServerAPI");

        builder.Services.AddMemoryCache();
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents();

        builder.Services.RegisterServicesFromInfiniLoreServerServices();
        builder.Services.RegisterServicesFromInfiniLoreServerClientShared();
        builder.Services.RegisterServicesFromInfiniLoreServerApi();

        builder.Services.AddLucideIcons();

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

        // Reference the library containing the static files
        var embeddedProvider = new EmbeddedFileProvider(
            typeof(IEntryPointInfiniLoreServerClientShared).Assembly,// Replace with a type from the external library
            "InfiniLore.ServerClient.Shared.wwwroot"// The root path defined in the library
        );

        app.UseStaticFiles(new StaticFileOptions {
                FileProvider = embeddedProvider,
                RequestPath = ""
            }
        );

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

        app.MapGet("/account/token", handler: async (IHttpContextAccessor httpContextAccessor, IJwtTokenEncoder tokenEncoder) => {
            if (httpContextAccessor.HttpContext is null || !httpContextAccessor.HttpContext.User.Identity!.IsAuthenticated)
                return Results.Unauthorized();

            // Retrieve the token using the access_token property (Auth0 integration)
            string? accessToken = await httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            if (accessToken.IsNullOrEmpty()) return Results.Unauthorized();

            // Token expiration logic (Auth0 provides token expiration info)
            if (!tokenEncoder.TryGetTokenUtcExpiry(accessToken, out DateTime expiresAt)) return Results.Unauthorized();

            // Refresh is handled by Auth0 middleware
            if (DateTime.UtcNow >= expiresAt) return Results.Unauthorized();

            return Results.Json(new TokenResponse {
                Token = accessToken,
                ExpiresAt = expiresAt.ToString("o")// ISO 8601 format for JS Date parsing
            });
        }).RequireAuthorization();
        #endregion

        app.UseFastEndpoints(config => {
            config.Endpoints.RoutePrefix = "api/v1";
            config.Errors.UseProblemDetails();

            config.Security.PermissionsClaimType = "permissions";
            config.Security.NameClaimType = ClaimTypes.NameIdentifier;
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
