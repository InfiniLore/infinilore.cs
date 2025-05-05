// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
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
public static class ContentDbFactory {
    private static readonly ILoggerFactory EmptyLoggerFactory = LoggerFactory.Create(builder => builder.AddSerilog(Log.Logger));
    private static Action<ModelBuilder>? _modelConfigurationAction;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static async Task<string> CreateDockerMsSqlContainer() {
        ILogger logger = EmptyLoggerFactory.CreateLogger("DOCKER mssql");

        MsSqlContainer container = new MsSqlBuilder()
            .WithPortBinding(40626, MsSqlBuilder.MsSqlPort)
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

    public static void RegisterDatabase(
        IServiceCollection services, 
        Action<DbContextOptionsBuilder> optionsAction,
        Action<ModelBuilder>? modelConfigurationAction = null) 
    {
        _modelConfigurationAction = modelConfigurationAction;
        
        services.AddDbContextFactory<ContentDb>(options => {
            ILoggerFactory databaseLoggerFactory = LoggerFactory.Create(builder =>
                builder.AddSerilog(Log.Logger.ForContext("Section", "EFCORE ContentDb"))
            );

            options.UseLoggerFactory(databaseLoggerFactory);
            optionsAction.Invoke(options);
        });

        // Our UnitOfWork is integral to the correct execution of the repo pattern
        services.AddReadonlyUnitOfWork<ContentDb>();
        services.RegisterServicesFromInfiniLoreServerDatabase();
    }
    
    internal static void ConfigureModel(ModelBuilder modelBuilder) {
        _modelConfigurationAction?.Invoke(modelBuilder);
    }
}
