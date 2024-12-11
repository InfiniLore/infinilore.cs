// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using InfiniLore.Database.Models.Content.Account;
using InfiniLore.Database.MsSqlServer;
using InfiniLore.Database.Repositories;
using InfiniLore.Server.Contracts.Database;
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
public class DatabaseInfrastructure : IAsyncInitializer, IAsyncDisposable {
    private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-CU10-ubuntu-22.04")
        .Build();

    public MsSqlDbContext DbContext { get; }
    public IServiceProvider ServiceProvider { get; }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public DatabaseInfrastructure() {
        var services = new ServiceCollection();

        _msSqlContainer.StartAsync().Wait();

        services.AddSingleton(Logger.None);
        services.AddDbContextFactory<MsSqlDbContext>(
            options => options.UseSqlServer(_msSqlContainer.GetConnectionString())
        );

        services.AddIdentityCore<InfiniLoreUser>(options => {
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<MsSqlDbContext>()
            .AddSignInManager();

        // Register the services which access and manipulate the database.
        services.RegisterServicesFromInfiniLoreDatabaseMsSqlServer();
        services.RegisterServicesFromInfiniLoreDatabaseRepositories();

        ServiceProvider = services.BuildServiceProvider().CreateScope().ServiceProvider;

        MsSqlDbContext db = ServiceProvider.GetRequiredService<IDbUnitOfWork<MsSqlDbContext>>()
            .GetDbContextAsync().GetAwaiter().GetResult();

        DbContext = db;
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async Task InitializeAsync() {
        await DbContext.Database.EnsureCreatedAsync();
        await DbContext.SaveChangesAsync();
    }

    public async ValueTask DisposeAsync() {
        await _msSqlContainer.DisposeAsync();
        await DbContext.DisposeAsync();
    }
}