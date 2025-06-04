// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using DataSources.InfiniLore.Server;
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Modules.LoreScopes.Server.Database;
using InfiniLore.Server.Modules.LoreScopes.Database;

namespace Tests.InfiniLore.Modules.LoreScopes.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[ClassDataSource<ServiceProviderDataSource>(Shared = SharedType.PerTestSession)]
public class LoreScopeRepositoryTests(ServiceProviderDataSource serviceProvider) {
    private IUnitOfWorkFactory Factory => serviceProvider.GetRequiredService<IUnitOfWorkFactory>();
    private GuidStore GuidStore => serviceProvider.GetRequiredService<GuidStore>();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Test Methods
    // -----------------------------------------------------------------------------------------------------------------
    [Test]
    public async Task BoundToCorrectRepository() {
        // Arrange
        await using IUnitOfWork unitOfWork = Factory.Create();

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
        await using IUnitOfWork unitOfWork = Factory.Create();
        var repo = await unitOfWork.GetRepositoryAsync<ILoreScopeRepository>();

        // Act
        RepoOutcome result = await repo.IsNameTakenAsync(name, GuidStore.GetGuid(userIdSeed));

        // Assert
        await Assert.That(result.TryGetAsState(out bool isTaken)).IsTrue();
        await Assert.That(isTaken).IsEqualTo(expected);
    }

    [Test]
    [Arguments("KNOWN NAME", 2, false)]
    [Arguments("unknown", 2, true)]
    [Arguments("KNOWN NAME", 1, true)]// Same name, but different user
    public async Task IsLoreScopeNotNameTakenAsync_ShouldReturnExpected(string name, int userIdSeed, bool expected) {
        // Arrange
        await using IUnitOfWork unitOfWork = Factory.Create();
        var repo = await unitOfWork.GetRepositoryAsync<ILoreScopeRepository>();

        // Act
        RepoOutcome result = await repo.IsNameNotTakenAsync(name, GuidStore.GetGuid(userIdSeed));

        // Assert
        await Assert.That(result.TryGetAsState(out bool isTaken)).IsTrue();
        await Assert.That(isTaken).IsEqualTo(expected);
    }
}
