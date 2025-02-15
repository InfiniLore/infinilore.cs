// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Database.Models.Content.Account;
using InfiniLore.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Core;
using Testcontainers.MsSql;
using TUnit.Core.Interfaces;

namespace Tests.InfiniLore.Database.Repositories.TestInfrastructure;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class DatabaseInfrastructure : IAsyncInitializer {
    private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-CU10-ubuntu-22.04")
        .Build();
    public IServiceProvider ServiceProvider { get; }

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public DatabaseInfrastructure() {
        var services = new ServiceCollection();

        _msSqlContainer.StartAsync().Wait();

        services.AddSingleton(Logger.None);
        services.AddDbContextFactory<ContentDbContext>(
            options => options
                .UseSqlServer(_msSqlContainer.GetConnectionString())
                .EnableSensitiveDataLogging()
        );

        services.AddIdentityCore<InfiniLoreUser>(options => {
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<ContentDbContext>()
            .AddSignInManager();

        // Register the services which access and manipulate the database.
        services.AddUnitOfWork<ContentDbContext>();
        services.RegisterServicesFromInfiniLoreDatabase();

        ServiceProvider = services.BuildServiceProvider();
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async Task InitializeAsync() {
        var factory = ServiceProvider.GetRequiredService<IUnitOfWorkFactory>();
        await using IUnitOfWork unitOfWork = factory.Create();
        
        var db = await unitOfWork.GetDbContextAsync<ContentDbContext>();
        await db.Database.EnsureCreatedAsync();
        await db.SaveChangesAsync();
    }
}
