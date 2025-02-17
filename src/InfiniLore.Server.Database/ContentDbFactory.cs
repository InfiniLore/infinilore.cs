// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.AspNetCore;
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Server.Database.Models.Account;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Testcontainers.MsSql;

namespace InfiniLore.Server.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// The ContentDbFactory class provides functionality to create and manage the configuration of
/// a database container and register the database services for a web application.
/// </summary>
public static class ContentDbFactory {
    /// Creates and starts a Docker container instance configured for an MSSQL database.
    /// The method sets up the container with specific configurations such as port bindings,
    /// image, password, container name, logging, and reuse policies. Once the container is
    /// started, it retrieves and returns the database connection string.
    /// <returns>
    /// A string containing the connection string to the started MSSQL Docker container.
    /// </returns>
    public static async Task<string> CreateDockerMsSqlContainer() {
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
        ILogger logger = containerLoggerFactory.CreateLogger("ContentDbFactory");
        logger.LogInformation("Database connection string: {ConnectionString}", container.GetConnectionString());
        
        return container.GetConnectionString();
    }

    /// <summary>
    /// Registers the database context and related services for the application.
    /// </summary>
    /// <param name="builder">
    /// The web application builder used to configure and build the application.
    /// </param>
    /// <param name="optionsAction">
    /// An action to configure the database context options.
    /// </param>
    public static void RegisterDatabase(WebApplicationBuilder builder, Action<DbContextOptionsBuilder> optionsAction) {
        builder.Services.AddDbContextFactory<ContentDb>(options => {
            ILoggerFactory databaseLoggerFactory = LoggingFactoryExtensions.CreateWithSerilog("EFCORE ContentDb");
            options.UseLoggerFactory(databaseLoggerFactory);
            
            optionsAction.Invoke(options);
        });
        
        builder.Services.AddIdentityCore<InfiniLoreUser>(options => {
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<ContentDb>()
            .AddSignInManager()
            .AddRoleManager<RoleManager<IdentityRole<Guid>>>();

        builder.Services.AddUnitOfWork<ContentDb>();
        builder.Services.AddUnitOfWork<ContentDb>("ContentDb");
        
        builder.Services.AddIdentityApiEndpoints<InfiniLoreUser>();
    }
}
