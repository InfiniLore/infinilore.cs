// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Bogus;
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories.Data.System;
using InfiniLore.Server.Database;
using InfiniLore.Server.Database.Models.Data.System;
using InfiniLore.Server.Database.Repositories.Data.System;
using Microsoft.EntityFrameworkCore;
using Tests.InfiniLore.Server.Database.DataSources;
using Tests.InfiniLore.Server.Database.Stores;

namespace Tests.InfiniLore.Server.Database.Repositories.Data.System;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[ClassDataSource<ContentDbInfrastructure>(Shared = SharedType.PerTestSession)]
public class KeyValueStoreRepositoryTests(ContentDbInfrastructure infrastructure) {
    [Before(Test)]
    public async Task SeedDatabase() {
        var dbContext = await infrastructure.GetDbContextAsync<ContentDb>();
        if (await dbContext.KeyValueStores.AnyAsync()) return;

        try {
            await dbContext.KeyValueStores.AddAsync(KeyValueStoreFaker.Entry001);
            await dbContext.SaveChangesAsync();
        }
        catch {
            // ignored
        }
    }

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
    public async Task TryAddAsync_ReturnsExpectedResult() {
        // Arrange
        await using IUnitOfWork unitOfWork = await infrastructure.GetUnitOfWork();
        var repo = await unitOfWork.GetRepositoryAsync<IKeyValueStoreRepository>();
        var dbContext = await unitOfWork.GetDbContextAsync<ContentDb>();
        
        var guid = Guid.NewGuid();
        const string key = "key-test";
        const string value = "value-test";
        var model = new KeyValueStore {
            Id = guid,
            Key = key,
            Value = value
        };
        
        // Act
        RepoResult result = await repo.TryAddAsync(model);
        dbContext.ChangeTracker.Clear();
        KeyValueStore? actual = await dbContext.KeyValueStores.FirstOrDefaultAsync(x => x.Id == guid);
        
        // Assert
        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(actual).IsNotNull()
            .And.HasMember(m => m!.Id).EqualTo(guid)
            .And.HasMember(m => m!.Key).EqualTo(key)
            .And.HasMember(m => m!.Value).EqualTo(value);
    }

    [Test]
    public async Task TryAddAsync_ReturnsFailure() {
        // Arrange
        await using IUnitOfWork unitOfWork = await infrastructure.GetUnitOfWork();
        var repo = await unitOfWork.GetRepositoryAsync<IKeyValueStoreRepository>();
        KeyValueStore modelWithSameId = KeyValueStoreFaker.EntryWithFixedId(KeyValueStoreFaker.Entry001Id);

        // Act
        RepoResult result = await repo.TryAddAsync(modelWithSameId);

        // Assert
        await Assert.That(result)
            .HasMember(m => m.IsFailure).EqualTo(true)
            .And.HasMember(m => m.IsSuccess).EqualTo(false)
            .And.HasMember(m => m.AsFailure.IsKnownFailure).EqualTo(true)
            .And.HasMember(m => m.AsFailure.IsUnknownFailure).EqualTo(false)
            .And.HasMember(m => m.AsFailure.AsKnownFailure).EqualTo(RepositoryFailures.ModelFailedUniqueConstraint);
    }

}
