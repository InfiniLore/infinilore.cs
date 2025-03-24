// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using CodeOfChaos.Types;
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Server.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.DataSeeder.Seeders;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<DatabaseMigrator>(ServiceLifetime.Scoped)]
public class DatabaseMigrator(IUnitOfWorkFactory factory, IReadonlyUnitOfWorkFactory readonlyUnitOfWorkFactory, ILogger<DatabaseMigrator> logger) : Seeder {

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task<bool> ShouldSeedAsync(CancellationToken ct = default) {
        await using IReadonlyUnitOfWork unitOfWork = readonlyUnitOfWorkFactory.Create();
        await using var db = await unitOfWork.GetDbContextAsync<ContentDb>(ct);

        try {
            IEnumerable<string> pendingMigrations = await db.Database.GetPendingMigrationsAsync(ct);
            bool shouldSeed = pendingMigrations.Any();

            logger.Information("Database Migration state is {State}", shouldSeed);
            return shouldSeed;
        }
        catch (Exception e) {
            // If a complicated error happens, this is usually due to the db not having any migrations yet
            logger.Error(e, "Error while checking database migration state");
            return true;
        }
    }

    public override async Task SeedAsync(CancellationToken ct = new()) {
        await using IUnitOfWork unitOfWork = factory.Create();
        await using var db = await unitOfWork.GetDbContextAsync<ContentDb>(ct);
        logger.Information("Starting ContentDb Database migration...");

        await db.Database.MigrateAsync(cancellationToken: ct);
        await db.SaveChangesAsync(ct);
        logger.Information("Finished ContentDb Database migration.");
    }
}
