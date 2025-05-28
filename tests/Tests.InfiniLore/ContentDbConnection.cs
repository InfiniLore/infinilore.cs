// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using DataSources.InfiniLore.Server;
using InfiniLore.Server.Database;
using Microsoft.EntityFrameworkCore;

namespace Tests.InfiniLore;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[DiDataSource]
public class ContentDbConnection(IReadonlyUnitOfWorkFactory readonlyUnitOfWorkFactory) {
    [Test]
    public async Task CanConnect() {
        // Arrange
        await using IReadonlyUnitOfWork unitOfWork = readonlyUnitOfWorkFactory.Create();
        var dbContext = await unitOfWork.GetDbContextAsync<ContentDb>();

        // Act
        bool canConnect = await dbContext.Database.CanConnectAsync();

        // Assert
        await Assert.That(canConnect).IsTrue();
    }

    [Test]
    public async Task IsMigratedCorrectly() {
        // Arrange
        await using IReadonlyUnitOfWork unitOfWork = readonlyUnitOfWorkFactory.Create();
        var dbContext = await unitOfWork.GetDbContextAsync<ContentDb>();

        // Act
        string[] allMigrations = dbContext.Database.GetMigrations().ToArray();
        string[] appliedMigrations = (await dbContext.Database.GetAppliedMigrationsAsync()).ToArray();

        // Assert
        await Assert.That(allMigrations).IsNotEmpty();
        await Assert.That(appliedMigrations).IsNotEmpty();
        await Assert.That(allMigrations).IsEquivalentTo(appliedMigrations);
    }
}
