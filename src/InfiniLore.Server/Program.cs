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
using InfiniLore.Modules.Core.Server;
using InfiniLore.Modules.Core.Server.ApiEndpoints;
using InfiniLore.Modules.Core.Server.Auth;
using InfiniLore.Modules.Core.Server.Encryption;
using InfiniLore.Modules.Core.Server.TokenStore;
using InfiniLore.Modules.Core.Shared;
using InfiniLore.Server.Components;
using InfiniLore.Server.Database;
using InfiniLore.Modules.LoreScopes.Server;
using InfiniLore.Modules.LsMarkdownFiles.Server;
using InfiniLore.Server.Cli;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Security.Claims;

namespace InfiniLore.Server;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class Program {
    public static async Task<int> Main(string[] args) {
        return await GlobalExceptionHandler.ExecuteWithGlobalExceptionHandlingAsync(async () => {
            WebApplicationBuilder builder = CreateBuilder(args);
            
            // Technically, we need to wrap this as a `IsDevelopment`, but that will be for a later stage
            //      This is required to run right here, as it defines some configuration required for the connectionStrings 
            await using var devEnv = InfiniLoreContainers.CreateForDevelopment();
            await devEnv.InitializeAsync();
            devEnv.AddToConfiguration(builder.Configuration);
            
            WebApplication app = BuildApp(builder);
            
            if (!args.IsEmpty()) {
                bool shouldExit = await ExecuteCliCommands(app, args);
                if (shouldExit) return 200;
            }
            
            await Start(app);
            return 0;
        });
    }

    private static WebApplicationBuilder CreateBuilder(string[] args) {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        builder.OverrideLoggingWithSerilog(config => config
            .AsAnnaSasDevServerConsole(
                24,
                configure: asyncConsoleConfig => asyncConsoleConfig.ApplyThemeToRedirectedOutput = true// Needed for nice DotnetWatch console output    
            )
            .WithTruncateSourceContextEnricher(maxLength: 24)
        );
        return builder;
    }
    
    private static async Task<bool> ExecuteCliCommands(WebApplication app, string[] args) {
        ICliParser parser = CliParser.CreateBuilder()
            .WithServiceProvider(() => app.Services)
            .AddFromAssembly<IServerEntry>()
            .Build();
        
        await parser.ExecuteAsync(args);
        var cliPostRunStatus = app.Services.GetRequiredService<CliPostRunEffects>();
        if (!cliPostRunStatus.ShouldExit) return false;

        // If we get here, we should exit with a specific code
        var logger = app.Services.GetRequiredService<ILogger<CliParser>>();
        logger.Information("Exit requested by CLI tool");
        return true;
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Builder
    // -----------------------------------------------------------------------------------------------------------------
    private static WebApplication BuildApp(WebApplicationBuilder builder) {
        ServerModuleBuilder moduleBuilder = ServerModuleBuilder.Create(builder)
            .AddModule<IServerModuleEntryCore>()
            .AddModule<IServerModuleEntryLoreScopes>()
            .AddModule<IModulelsLsMarkdownFilesServer>();
        
        #region Database
        ContentDbFactory.RegisterDatabase(
            builder.Services,
            moduleBuilder.ModuleAssemblies, 
            options => options.UseSqlServer(builder.Configuration["ConnectionStrings:SqlServer"])
        );

        S3FileDbFactory.RegisterDatabase(
            builder.Services,
            builder.Configuration["ConnectionStrings:Minio:Endpoint"],
            builder.Configuration["ConnectionStrings:Minio:AccessKey"],
            builder.Configuration["ConnectionStrings:Minio:SecretKey"]
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
            .AddPolicy(ApiPolicies.JwtProtected, configurePolicy: policy => {
                policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireAuthenticatedUser();// Enforce authentication
            });

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
        app.UseAntiforgery();

        app.UseAuthentication();
        app.UseAuthorization();

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
