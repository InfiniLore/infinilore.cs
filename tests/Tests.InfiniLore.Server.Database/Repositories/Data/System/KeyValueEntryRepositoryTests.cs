// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using DataSources.InfiniLore.Server;
using Fakers.InfiniLore.Server;
using InfiniLore.Server.Database;
using InfiniLore.Server.Modules.Core.Database;
using Microsoft.EntityFrameworkCore;

namespace Tests.InfiniLore.Server.Database.Repositories.Data.System;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[ClassDataSource<ContentDbInfrastructure, KeyValueEntryFaker>(Shared = [SharedType.PerTestSession, SharedType.PerClass])]
public class KeyValueEntryRepositoryTests(ContentDbInfrastructure infrastructure, KeyValueEntryFaker faker) {
    // -----------------------------------------------------------------------------------------------------------------
    // Test Methods
    // -----------------------------------------------------------------------------------------------------------------
    [Test]
    public async Task BoundToCorrectRepository() {
        // Arrange
        await using IUnitOfWork unitOfWork = await infrastructure.GetUnitOfWork();

        // Act
        var repo = await unitOfWork.GetRepositoryAsync<IKeyValueEntryRepository>();

        // Assert
        await Assert.That(repo).IsTypeOf<KeyValueEntryRepository>();
    }

    [Test]
    public async Task TryAddOrUpdateAsync_ReturnsExpectedResult() {
        // Arrange
        await using IUnitOfWork unitOfWork = await infrastructure.GetUnitOfWork();
        var repo = await unitOfWork.GetRepositoryAsync<IKeyValueEntryRepository>();
        var dbContext = await unitOfWork.GetDbContextAsync<ContentDb>();

        KeyValueEntryModel model = faker.Faker.Generate();
        string key = model.Key;
        string? value = model.Value;

        // Act
        Result result = await repo.TryAddOrUpdateAsync(model);
        dbContext.ChangeTracker.Clear();
        
        DbSet<KeyValueEntryModel> keyValueEntries = dbContext.Set<KeyValueEntryModel>();
        KeyValueEntryModel? actual = await keyValueEntries.FirstOrDefaultAsync(x => x.Key == key);

        // Assert
        await Assert.That(result.IsState).IsTrue();
        await Assert.That(actual).IsNotNull()
            .And.HasMember(m => m!.Key).EqualTo(key)
            .And.HasMember(m => m!.Value).EqualTo(value);
    }
}
