// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using DataSources.InfiniLore.Server;
using Fakers.InfiniLore.Server;
using InfiniLore.Server.Contracts.Database.Repositories.Data.User;
using InfiniLore.Server.Database.Repositories.Data.User;

namespace Tests.InfiniLore.Server.Database.Repositories.Data.User;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[ClassDataSource<ContentDbInfrastructure, LoreScopeFaker, GuidStore>(Shared = [SharedType.PerTestSession, SharedType.PerClass])]
public class LoreScopeRepositoryTests(ContentDbInfrastructure infrastructure, LoreScopeFaker faker, GuidStore guidStore) {
    [Test]
    public async Task BoundToCorrectRepository() {
        // Arrange
        await using IUnitOfWork unitOfWork = infrastructure.GetReadonlyUnitOfWork();

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
        await using IUnitOfWork unitOfWork = infrastructure.GetReadonlyUnitOfWork();
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
        await using IUnitOfWork unitOfWork = infrastructure.GetReadonlyUnitOfWork();
        var repo = await unitOfWork.GetRepositoryAsync<ILoreScopeRepository>();

        // Act
        Result result = await repo.IsLoreScopeNameNotTakenAsync(name, guidStore.GetGuid(userIdSeed));

        // Assert
        await Assert.That(result.TryGetState(out bool isTaken)).IsTrue();
        await Assert.That(isTaken).IsEqualTo(expected);
    }
}
