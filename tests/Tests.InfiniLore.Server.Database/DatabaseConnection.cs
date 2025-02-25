// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Server.Database;
using Microsoft.EntityFrameworkCore;
using Tests.InfiniLore.Server.Database.DataSources;

namespace Tests.InfiniLore.Server.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[ClassDataSource<ContentDbInfrastructure>(Shared = SharedType.PerTestSession)]
public class DatabaseConnection(ContentDbInfrastructure infrastructure) {
    [Test]
    public async Task CanConnect() {
        // Arrange
        await using IUnitOfWork unitOfWork = await infrastructure.GetUnitOfWork();
        var dbContext = await unitOfWork.GetDbContextAsync<ContentDb>();

        // Act
        bool canConnect = await dbContext.Database.CanConnectAsync();

        // Assert
        await Assert.That(canConnect).IsTrue();
    }

    [Test]
    public async Task IsMigratedCorrectly() {
        // Arrange
        await using IUnitOfWork unitOfWork = await infrastructure.GetUnitOfWork();
        var dbContext = await unitOfWork.GetDbContextAsync<ContentDb>();

        // Act
        var allMigrations = dbContext.Database.GetMigrations().ToArray();
        var appliedMigrations = (await dbContext.Database.GetAppliedMigrationsAsync()).ToArray();
        
        // Assert
        await Assert.That(allMigrations).IsNotEmpty();
        await Assert.That(appliedMigrations).IsNotEmpty();
        await Assert.That(allMigrations).IsEquivalentTo(appliedMigrations);
    }
}
