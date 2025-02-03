// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using CodeOfChaos.Types;
using InfiniLore.Database.MsSqlServer;
using InfiniLore.Server.Contracts.Database;
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
        await using var db = await unitOfWork.GetDbContextAsync<MsSqlDbContext>(ct);
        _logger.Information("Starting Database migration...");

        await db.Database.MigrateAsync(cancellationToken: ct);
        await db.SaveChangesAsync(ct);
        _logger.Information("Finished Database migration.");
    }
}
