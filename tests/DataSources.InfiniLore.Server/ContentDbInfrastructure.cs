// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.AspNetCore;
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Server.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Testcontainers.MsSql;
using TUnit.Core.Interfaces;

namespace DataSources.InfiniLore.Server;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ContentDbInfrastructure : IAsyncInitializer, IAsyncDisposable {
    private string? ConnectionString { get; set; }
    private MsSqlContainer? Container { get; set; }
    private IServiceProvider? ServiceProvider { get; set; }

    public async ValueTask DisposeAsync() {
        if (Container is not null) {
            await Container.StopAsync();
            await Container.DisposeAsync();
        }

        ConnectionString = null;
        Container = null;

        GC.SuppressFinalize(this);
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async Task InitializeAsync() {
        #region Setup DbConnection
        ILoggerFactory containerLoggerFactory = LoggingFactoryExtensions.CreateWithSerilog("TEST docker");
        Container = new MsSqlBuilder()
            .WithLogger(containerLoggerFactory.CreateLogger<MsSqlContainer>())
            .WithImage("mcr.microsoft.com/mssql/server:2022-CU10-ubuntu-22.04")
            .WithName("infinilore-testing-db")
            .Build();

        await Container.StartAsync();
        ILogger logger = containerLoggerFactory.CreateLogger("ContentDbFactory");
        logger.LogInformation("Database connection string: {ConnectionString}", Container.GetConnectionString());

        ConnectionString = Container.GetConnectionString();
        #endregion

        var services = new ServiceCollection();
        services.AddLogging();
        ContentDbFactory.RegisterDatabase(services, optionsAction: builder => {
            builder.UseSqlServer(ConnectionString);
        });

        ServiceProvider = services.BuildServiceProvider();

        await using (ContentDb dbContext = await ServiceProvider.GetRequiredService<IDbContextFactory<ContentDb>>().CreateDbContextAsync()) {
            await dbContext.Database.MigrateAsync();
            await dbContext.SaveChangesAsync();
        }

        var populator = new ContentDbPopulator(ServiceProvider);
        await populator.PopulateAsync();
    }

    public async Task<IUnitOfWork> GetUnitOfWork() {
        if (ServiceProvider is null) throw new InvalidOperationException("Service provider is not initialized.");

        IUnitOfWork unitOfWork = ServiceProvider.GetRequiredService<IUnitOfWorkFactory>().Create();
        await unitOfWork.TryCreateTransactionAsync();
        return unitOfWork;
    }

    public IReadonlyUnitOfWork GetReadonlyUnitOfWork() {
        if (ServiceProvider is null) throw new InvalidOperationException("Service provider is not initialized.");
        IReadonlyUnitOfWork unitOfWork = ServiceProvider.GetRequiredService<IReadonlyUnitOfWorkFactory>().Create();
        return unitOfWork;
    }

    public async Task<T> GetDbContextAsync<T>() where T : DbContext {
        if (ServiceProvider is null) throw new InvalidOperationException("Service provider is not initialized.");

        var factory = ServiceProvider.GetRequiredService<IDbContextFactory<T>>();
        return await factory.CreateDbContextAsync();
    }
}
