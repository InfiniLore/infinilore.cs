// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using InfiniLore.Database.MsSqlServer;
using InfiniLore.Server.Contracts.Database;
using Microsoft.Extensions.DependencyInjection;
using Tests.InfiniLore.Database.Repositories.TestInfrastructure;

namespace Tests.InfiniLore.Database.Repositories;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[ClassDataSource<DatabaseInfrastructure>(Shared = SharedType.PerTestSession)]
public class LoreScopeCommandRepositoryTest(DatabaseInfrastructure infrastructure) {
    private readonly IDbUnitOfWork<MsSqlDbContext> _unitOfWork = infrastructure.ServiceProvider.GetRequiredService<IDbUnitOfWork<MsSqlDbContext>>();

    [Test]
    public async Task TestCanConnect() {
        // Arrange: get dbContext
        var dbContext = await _unitOfWork.GetDbContextAsync();

        // Act: check the connection
        var canConnect = await dbContext.Database.CanConnectAsync();

        // Assert: verify connection success
        await Assert.That(canConnect).IsTrue();
    }
}
