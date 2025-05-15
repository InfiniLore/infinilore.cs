// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.CliArgsParser;
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Server.Database;
using InfiniLore.Server.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Cli.DbMigrations;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[CliData("database-migrate")]
public partial class MigrateDatabaseCommand(
    ILogger<MigrateDatabaseCommand> logger,
    IReadonlyUnitOfWorkFactory readonlyUnitOfWorkFactory,
    ICliPostRunEffects cliPostRunStatus
) : ICliCommand<MigrateDatabaseParameters> {
    public async ValueTask ExecuteAsync(MigrateDatabaseParameters parameters, CancellationToken ct = new()) {
        await using IReadonlyUnitOfWork unitOfWork = readonlyUnitOfWorkFactory.Create();
        await using var db = await unitOfWork.GetDbContextAsync<ContentDb>(ct);

        if (!await HasPendingMigrationsAsync(db, ct)) {
            logger.Warning("Could not find migrations to apply");
            cliPostRunStatus.ShouldExit = true;
            return;
        }
        
        if (!await TryExecuteMigrations(db, ct)) {
            logger.Error("Error while applying migrations");
            cliPostRunStatus.ShouldExit = true;
            return;
        }

        // Check if there are any pending migrations after the migration
        if (await HasPendingMigrationsAsync(db, ct)) {
            logger.Warning("Could not apply all pending migrations.");
            cliPostRunStatus.ShouldExit = true;
            return;
        }

        logger.Information("Successfully applied all pending migrations.");
        cliPostRunStatus.ShouldExit = true;
    }
    
    private async ValueTask<bool> HasPendingMigrationsAsync(ContentDb db, CancellationToken ct) {
        try {
            IEnumerable<string> pendingMigrations = await db.Database.GetPendingMigrationsAsync(ct);
            int migrationCount = pendingMigrations.Count();
            logger.Information("found {migrationCount} Migrations to apply", migrationCount);
            return migrationCount > 0;
        }
        catch (Exception e) {
            // If a complicated error happens, this is usually due to the db not having any migrations yet
            logger.Error(e, "Error while checking database migration state");
            return false;
        }
    }

    private async ValueTask<bool> TryExecuteMigrations(ContentDb db, CancellationToken ct) {
        try {
            await db.Database.MigrateAsync(cancellationToken: ct);
            int result = await db.SaveChangesAsync(ct);
            logger.Information("Finished {db} Database migration.", nameof(ContentDb));
            return result > 0;
        }
        catch (Exception e) {
            logger.Error(e, "Error while migrating ContentDb Database.");
            return false;
        }
    } 
}
