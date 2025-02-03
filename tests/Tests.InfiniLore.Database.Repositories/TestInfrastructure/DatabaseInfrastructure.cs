// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Database.Models.Content.Account;
using InfiniLore.Database.MsSqlServer;
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

        ServiceProvider = services.BuildServiceProvider().CreateScope().ServiceProvider;
    }
    public IServiceProvider ServiceProvider { get; }

    public async ValueTask DisposeAsync() {
        await _msSqlContainer.DisposeAsync();
        GC.SuppressFinalize(this);
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async Task InitializeAsync() {
        var db = await ServiceProvider.GetRequiredService<IUnitOfWork>().GetDbContextAsync<MsSqlDbContext>();
        await db.Database.EnsureCreatedAsync();
        await db.SaveChangesAsync();
    }
}
