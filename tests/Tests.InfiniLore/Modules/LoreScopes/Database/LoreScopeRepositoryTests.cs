// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using DataSources.InfiniLore.Server;
using InfiniLore.Server.Modules.LoreScopes.Database;

namespace Tests.InfiniLore.Modules.LoreScopes.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class LoreScopeRepositoryTests {
    [ClassDataSource<ServiceProviderDataSource>(Shared = SharedType.PerAssembly)]
    public required ServiceProviderDataSource ServiceProvider { get; init; }
    
    [Test]
    public async Task BoundToCorrectRepository() {
        // Arrange
        var unitOfWorkFactory = ServiceProvider.GetRequiredService<IUnitOfWorkFactory>();
        await using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();

        // Act
        var repo = await unitOfWork.GetRepositoryAsync<ILoreScopeRepository>();

        // Assert
        await Assert.That(repo).IsTypeOf<LoreScopeRepository>();
    }

    [Test]
    [Arguments("KNOWN NAME", 2, true)]
    [Arguments("unknown", 2, false)]
    [Arguments("KNOWN NAME", 1, false)]// Same name, but different user
    public async Task IsLoreScopeNameTakenAsync_ShouldReturnExpected(string name, int userIdSeed, bool expected) {
        // Arrange
        var guidStore = ServiceProvider.GetRequiredService<GuidStore>();
        var unitOfWorkFactory = ServiceProvider.GetRequiredService<IUnitOfWorkFactory>();
        await using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();
        var repo = await unitOfWork.GetRepositoryAsync<ILoreScopeRepository>();

        // Act
        Result result = await repo.IsLoreScopeNameTakenAsync(name, guidStore.GetGuid(userIdSeed));

        // Assert
        await Assert.That(result.TryGetState(out bool isTaken)).IsTrue();
        await Assert.That(isTaken).IsEqualTo(expected);
    }

    [Test]
    [Arguments("KNOWN NAME", 2, false)]
    [Arguments("unknown", 2, true)]
    [Arguments("KNOWN NAME", 1, true)]// Same name, but different user
    public async Task IsLoreScopeNotNameTakenAsync_ShouldReturnExpected(string name, int userIdSeed, bool expected) {
        // Arrange
        var guidStore = ServiceProvider.GetRequiredService<GuidStore>();
        var unitOfWorkFactory = ServiceProvider.GetRequiredService<IUnitOfWorkFactory>();
        await using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();
        var repo = await unitOfWork.GetRepositoryAsync<ILoreScopeRepository>();

        // Act
        Result result = await repo.IsLoreScopeNameNotTakenAsync(name, guidStore.GetGuid(userIdSeed));

        // Assert
        await Assert.That(result.TryGetState(out bool isTaken)).IsTrue();
        await Assert.That(isTaken).IsEqualTo(expected);
    }
}
