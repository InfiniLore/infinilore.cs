// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Database;
using Microsoft.Extensions.DependencyInjection;
using Tests.InfiniLore.Database.Repositories.TestInfrastructure;

namespace Tests.InfiniLore.Database.Repositories;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[ClassDataSource<DatabaseInfrastructure>(Shared = SharedType.PerTestSession)]
public class LoreScopeCommandRepositoryTest(DatabaseInfrastructure infrastructure) {
    private readonly IUnitOfWork _unitOfWork = infrastructure.ServiceProvider.GetRequiredService<IUnitOfWork>();

    [Test]
    public async Task TestCanConnect() {
        // Arrange: get dbContext
        var dbContext = await _unitOfWork.GetDbContextAsync<ContentDbContext>();

        // Act: check the connection
        bool canConnect = await dbContext.Database.CanConnectAsync();

        // Assert: verify connection success
        await Assert.That(canConnect).IsTrue();
    }
}
