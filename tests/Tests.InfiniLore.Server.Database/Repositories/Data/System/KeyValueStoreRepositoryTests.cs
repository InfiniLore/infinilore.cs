// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using DataSources.InfiniLore.Server;
using Fakers.InfiniLore.Server;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories.Data.System;
using InfiniLore.Server.Database;
using InfiniLore.Server.Database.Models.Data.System;
using InfiniLore.Server.Database.Repositories.Data.System;
using Microsoft.EntityFrameworkCore;

namespace Tests.InfiniLore.Server.Database.Repositories.Data.System;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[ClassDataSource<ContentDbInfrastructure, KeyValueStoreFaker,GuidStore>(Shared = [SharedType.PerTestSession, SharedType.PerClass])]
public class KeyValueStoreRepositoryTests(ContentDbInfrastructure infrastructure, KeyValueStoreFaker faker, GuidStore guidStore) {
    // -----------------------------------------------------------------------------------------------------------------
    // Test Methods
    // -----------------------------------------------------------------------------------------------------------------
    [Test]
    public async Task BoundToCorrectRepository() {
        // Arrange
        await using IUnitOfWork unitOfWork = await infrastructure.GetUnitOfWork();

        // Act
        var repo = await unitOfWork.GetRepositoryAsync<IKeyValueStoreRepository>();

        // Assert
        await Assert.That(repo).IsTypeOf<KeyValueStoreRepository>();
    }

    [Test]
    public async Task TryAddOrUpdateAsync_ReturnsExpectedResult() {
        // Arrange
        await using IUnitOfWork unitOfWork = await infrastructure.GetUnitOfWork();
        var repo = await unitOfWork.GetRepositoryAsync<IKeyValueStoreRepository>();
        var dbContext = await unitOfWork.GetDbContextAsync<ContentDb>();

        const string key = "key-test";
        const string value = "value-test";
        var model = new KeyValueStore {
            Key = key,
            Value = value
        };

        // Act
        RepoResult result = await repo.TryAddOrUpdateAsync(model);
        dbContext.ChangeTracker.Clear();
        KeyValueStore? actual = await dbContext.KeyValueStores.FirstOrDefaultAsync(x => x.Key == key);

        // Assert
        await Assert.That(result.IsState).IsTrue();
        await Assert.That(actual).IsNotNull()
            .And.HasMember(m => m!.Key).EqualTo(key)
            .And.HasMember(m => m!.Value).EqualTo(value);
    }
}
