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
public class DatabaseMigrator(IUnitOfWorkFactory factory, ILoggerFactory loggerFactory) : Seeder {
    private readonly ILogger _logger = loggerFactory.CreateLogger("MIGRATE");

    public override async Task SeedAsync(CancellationToken ct = new()) {
        await using IUnitOfWork unitOfWork = factory.Create();
        await using var db = await unitOfWork.GetDbContextAsync<ContentDb>(ct);
        _logger.Information("Starting ContentDb Database migration...");

        await db.Database.MigrateAsync(cancellationToken: ct);
        await db.SaveChangesAsync(ct);
        _logger.Information("Finished ContentDb Database migration.");
    }
}
