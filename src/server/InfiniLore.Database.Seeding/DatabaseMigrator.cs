// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using CodeOfChaos.Types;
using CodeOfChaos.Types.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace InfiniLore.Database.Seeding;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<DatabaseMigrator>(ServiceLifetime.Scoped)]
public class DatabaseMigrator(IUnitOfWork unitOfWork, ILogger logger) : Seeder {
    private readonly ILogger _logger = logger.ForSectionProperty("MIGRATE mssql");

    public override async Task SeedAsync(CancellationToken ct = new()) {
        await using var db = await unitOfWork.GetDbContextAsync<ContentDbContext>(ct);
        _logger.Information("Starting Database migration...");

        await db.Database.MigrateAsync(cancellationToken: ct);
        await db.SaveChangesAsync(ct);
        _logger.Information("Finished Database migration.");
    }
}
