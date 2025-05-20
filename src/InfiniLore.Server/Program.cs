// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Auth0.AspNetCore.Authentication;
using CodeOfChaos.CliArgsParser;
using CodeOfChaos.Extensions.AspNetCore;
using FastEndpoints;
using FastEndpoints.Swagger;
using InfiniLore.Wasm;
using InfiniLore.Credentials.Auth0.DependencyInjection;
using InfiniLore.InfiniBlazor.Markdown.Config;
using InfiniLore.Server.Cli;
using InfiniLore.Server.Components;
using InfiniLore.Server.Database;
using InfiniLore.Server.Modules.Core;
using InfiniLore.Server.Modules.Core.ApiEndpoints;
using InfiniLore.Server.Modules.Core.Auth;
using InfiniLore.Server.Modules.Core.Encryption;
using InfiniLore.Server.Modules.Core.TokenStore;
using InfiniLore.Server.Modules.LoreScopes;
using InfiniLore.Server.Services;
using InfiniLore.Shared;
using InfiniLore.Shared.JwtToken;
using InfiniLore.Shared.Services.JwtToken;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Security.Claims;
using SharedAssemblyEntry = InfiniLore.Shared.IAssemblyEntry;

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
            if (!args.IsEmpty()) await ExecuteCliCommands(args, app); // Has to option to quit before starting of the app
            await Start(app);
        });
    }
    
    private static async Task ExecuteCliCommands(string[] args, WebApplication app) {
        ICliParser parser = CliParser.CreateBuilder()
            .WithServiceProvider(() => app.Services)
            .AddFromAssembly<ICliAssemblyEntrypoint>()
            .Build();
        
        await parser.ExecuteAsync(args);
        var cliPostRunStatus = app.Services.GetRequiredService<ICliPostRunEffects>();
        if (!cliPostRunStatus.ShouldExit) return;

        // If we get here, we should exit with a specific code
        var logger = app.Services.GetRequiredService<ILogger<CliParser>>();
        logger.Information("Exit requested by CLI tool");
        Environment.Exit(200);
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Builder
    // -----------------------------------------------------------------------------------------------------------------
    private static async Task<WebApplication> BuildApp(WebApplicationBuilder builder) {
        ServerModuleBuilder moduleBuilder = ServerModuleBuilder.Create(builder)
            .AddModule<IServerModuleEntryCore>()
            .AddModule<IServerModuleEntryLoreScopes>();
        
        #region Database
        // Technically we need to wrap this as a `IsDevelopment`
        //      And have another value for when we don't pull from our own container 
        //      Most of it is made through a factory which sets up the docker instance
        string connectionString = await ContentDbFactory.CreateDockerMsSqlContainer();
        ContentDbFactory.RegisterDatabase(
            builder.Services,
            moduleBuilder.ModuleAssemblies, 
            options => options.UseSqlServer(connectionString)
        );

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
        builder.Services.RegisterServicesFromInfiniLoreServerCli();
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

        builder.AddAuth0AccessTokenEncryptionOptions(); // Required to set options correctly
        #endregion

        #region FastEndpoints
        builder.Services.AddFastEndpoints(options => {
            options.DisableAutoDiscovery = true;

            options.Assemblies = moduleBuilder.ModuleAssemblies;
        });

        builder.Services.SwaggerDocument();
        #endregion

        #region InfiniBlazor
        builder.Services.AddInfiniBlazor(config => {
            config.AddMarkdownLogic(markdownConfig => markdownConfig.AddMarkdownParser<string, string>());
        });
        #endregion

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddHttpClient();
        builder.Services.AddHttpClient("ServerAPI");

        builder.Services.AddMemoryCache();
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents();
        
        builder.Services.RegisterServicesFromInfiniLoreServer();
        builder.Services.RegisterServicesFromInfiniLoreShared();

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
            typeof(SharedAssemblyEntry).Assembly,// Replace with a type from the external library
            "InfiniLore.Shared.wwwroot"// The root path defined in the library
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
