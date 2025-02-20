// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Server.Database.Models.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Testcontainers.MsSql;
using ILogger=Microsoft.Extensions.Logging.ILogger;

namespace InfiniLore.Server.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// The ContentDbFactory class provides functionality to create and manage the configuration of
/// a database container and register the database services for a web application.
/// </summary>
public static class ContentDbFactory {
    private static readonly ILoggerFactory EmptyLoggerFactory = LoggerFactory.Create(builder => builder.AddSerilog(Log.Logger));
    /// <summary>
    /// Creates and starts a Docker container instance configured for an MSSQL database.
    /// The method sets up the container with specific configurations such as port bindings,
    /// image, password, container name, logging, and reuse policies. Once the container is
    /// started, it retrieves and returns the database connection string.
    /// </summary>
    /// <returns>
    /// A string containing the connection string to the started MSSQL Docker container.
    /// </returns>
    public static async Task<string> CreateDockerMsSqlContainer() {
        ILogger logger = EmptyLoggerFactory.CreateLogger("DOCKER mssql");
        
        MsSqlContainer container = new MsSqlBuilder()
            .WithPortBinding(60426, MsSqlBuilder.MsSqlPort)
            .WithLogger(logger)
            .WithImage("mcr.microsoft.com/mssql/server:2022-CU10-ubuntu-22.04")
            .WithPassword("AnnaIsTrans4Ever!")
            .WithName("infinilore-development-db")
            .WithReuse(true)
            .WithLabel("reuse-id", "infinilore-development-db")
            .Build();

        await container.StartAsync();
        logger.LogInformation("Database connection string: {ConnectionString}", container.GetConnectionString());
        
        return container.GetConnectionString();
    }

    /// <summary>
    /// Registers the database context and related services for the application.
    /// </summary>
    /// <param name="services">the Webapp Service collection</param>
    /// <param name="optionsAction">
    /// An action to configure the database context options.
    /// </param>
    public static void RegisterDatabase(IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction) {
        services.AddDbContextFactory<ContentDb>(options => {
            ILoggerFactory databaseLoggerFactory = LoggerFactory.Create(builder => 
                builder.AddSerilog(Log.Logger.ForContext("Section", "EFCORE ContentDb"))
            );
            options.UseLoggerFactory(databaseLoggerFactory);
            
            optionsAction.Invoke(options);
        });
        
        services.AddIdentityCore<InfiniLoreUser>(options => {
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<ContentDb>()
            .AddSignInManager()
            .AddRoleManager<RoleManager<IdentityRole<Guid>>>();

        services.AddUnitOfWork<ContentDb>();
        services.AddUnitOfWork<ContentDb>("ContentDb");
        
        services.AddIdentityApiEndpoints<InfiniLoreUser>();
        
        services.RegisterServicesFromInfiniLoreServerDatabase();
    }
}
