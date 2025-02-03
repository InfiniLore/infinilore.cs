// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AspNetCore.Swagger.Themes;
using CodeOfChaos.Extensions.AspNetCore;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using InfiniLore.Database.Models.Content.Account;
using InfiniLore.Database.MsSqlServer;
using InfiniLore.Database.Seeding;
using InfiniLore.Database.Seeding.Content.Account;
using InfiniLore.Database.Seeding.Content.Data.System;
using InfiniLore.Server.API;
using InfiniLore.Server.Components;
using InfiniLore.Server.Services;
using InfiniLore.Server.Services.Authentication;
using InfiniLore.Server.Services.Authorization;
using InfiniLore.Server.Services.CQRS.PipelineBehaviours;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Claims;
using Testcontainers.MsSql;

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
        ILoggerFactory containerLoggerFactory = LoggingFactoryExtensions.CreateWithSerilog("CONTAINER mssqldb");
        MsSqlContainer container = new MsSqlBuilder()
            .WithPortBinding(60426, MsSqlBuilder.MsSqlPort)
            .WithLogger(containerLoggerFactory.CreateLogger<MsSqlContainer>())
            .WithImage("mcr.microsoft.com/mssql/server:2022-CU10-ubuntu-22.04")
            .WithPassword("AnnaIsTrans4Ever!")
            .WithName("infinilore-development-db")
            .WithReuse(true)
            .WithLabel("reuse-id", "infinilore-development-db")
            .Build();

        await container.StartAsync();
        Console.WriteLine($"Database connection string: {container.GetConnectionString()}");

        ILoggerFactory databaseLoggerFactory = LoggingFactoryExtensions.CreateWithSerilog("EFCORE mssqldb");
        builder.Services.AddDbContextFactory<MsSqlDbContext>(options =>
                options.UseSqlServer(container.GetConnectionString())
                    .UseLoggerFactory(databaseLoggerFactory)
            // .ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning))
        );
        
        builder.Services.AddIdentityCore<InfiniLoreUser>(options => {
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<MsSqlDbContext>()
            .AddSignInManager()
            .AddRoleManager<RoleManager<IdentityRole<Guid>>>();

        builder.Services.RegisterServicesFromInfiniLoreDatabaseMsSqlServer();// Registers the IUnitOfWorkDb<T>
        #endregion

        #region Authentication
        builder.Services.AddAuthenticationJwtBearer(
            signingOptions: options => {
                options.SigningKey = builder.Configuration["JWT:Key"];
            },
            bearerOptions: bearerOptions => {
                bearerOptions.TokenValidationParameters.RoleClaimType = ClaimTypes.Role;
                bearerOptions.TokenValidationParameters.NameClaimType = ClaimTypes.NameIdentifier;

                bearerOptions.TokenValidationParameters.ValidIssuer = builder.Configuration["JWT:Issuer"];
                bearerOptions.TokenValidationParameters.ValidAudience = builder.Configuration["JWT:Audience"];

                bearerOptions.MapInboundClaims = true;
            });

        builder.Services.AddAuthentication(o => {
            o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        });

        builder.Services.ConfigureApplicationCookie(
            cookieOptions => {
                // ReSharper disable once RedundantLambdaParameterType
                cookieOptions.Events.OnRedirectToLogin = (RedirectContext<CookieAuthenticationOptions> context) => {
                    if (IsApiRequest(context)) {
                        context.Response.StatusCode = 401;
                    }
                    else {
                        context.Response.Redirect(context.RedirectUri);
                    }

                    return Task.CompletedTask;
                };

                // ReSharper disable once RedundantLambdaParameterType
                cookieOptions.Events.OnRedirectToAccessDenied = (RedirectContext<CookieAuthenticationOptions> context) => {
                    if (IsApiRequest(context)) {
                        context.Response.StatusCode = 403;
                    }
                    else {
                        context.Response.Redirect(context.RedirectUri);
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

        #region API
        builder.Services
            .AddFastEndpoints(options => {
                options.Assemblies = [
                    typeof(IApiAssemblyEntry).Assembly
                ];
            })
            .SwaggerDocument(options => {
                options.DocumentSettings = settings => {
                    settings.Version = "v1";
                    settings.Title = "InfiniLore API";
                    settings.Description = "An ASP.NET Core Web API for managing InfiniLore";
                };
            });

        builder.Services.AddIdentityApiEndpoints<InfiniLoreUser>();
        #endregion

        #region MediatR
        builder.Services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssemblyContaining<Services.CQRS.Handlers.ICqrsHandlersAssemblyEntry>();
            cfg.AddInfinilorePipelineBehaviours();
        });
        #endregion
        
        #region Seeding
        // Don't forget to add the seeder classes to the service collection!
        builder.Services.RegisterServicesFromInfiniLoreDatabaseSeeding();
        builder.Services.AddOneTimeDataSeeder(seeder => {
            // Always start with migrating the DB if necessary
            //      I've debated a bit over if this is the correct location or not for this to happen
            //      In the end I've decided that this is a clear step in the "seeding" process of the server,
            //      and through a Seeder method overload we can also set up a system to ignore this seeder step if needed.
            seeder.AddSeeder<DatabaseMigrator>();

            // One SeederGroup has their seeders run in concurrency
            //      They do have their own scope, and thus their own dbContext
            //      This means they can execute data in "parallel" and therefor can't rely on each-other's data 
            seeder.AddSeederGroup(group => group
                .AddSeeder<RolesSeeder>()
                .AddSeeder<PermissionsSeeder>()
            );

            // User generation depends on a lot of things, and should thus come after "dependency-less" seeders
            seeder.AddSeeder<UserSeeder>();

            // // To ensure we don't forget one
            // seeder.AddRemainderSeedersAsOneGroup(typeof(AdvancedCSharp.Database.Seeding.IAssemblyEntry).Assembly);
        });
        #endregion

        builder.Services.RegisterServicesFromInfiniLoreServerServicesAuthorization();
        builder.Services.RegisterServicesFromInfiniLoreServerServicesAuthentication();
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

        app.MapStaticAssets();
        app.UseStaticFiles();
        app.UseAntiforgery();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
            .AddInteractiveWebAssemblyRenderMode()
            .AddAdditionalAssemblies(typeof(WasmClient.IAssemblyEntry).Assembly);

        app.UseDefaultExceptionHandler()
            .UseFastEndpoints(ctx => {
                ctx.Endpoints.RoutePrefix = "api/v1";
                ctx.Binding.ReflectionCache.AddFromInfiniLoreServerAPI();
                ctx.Errors.UseProblemDetails();
            });

        app.UseOpenApi();
        app.UseSwaggerUI(ModernStyle.Dark, setupAction: ctx => {
            ctx.SwaggerEndpoint("v1/swagger.json", "InfiniLore API v1");
            ctx.RoutePrefix = "swagger";
        });

        await app.RunAsync();
    }

    private static bool IsApiRequest(RedirectContext<CookieAuthenticationOptions> context) => context is { Request.Path.Value: "/api", Response.StatusCode: 200 };
}
