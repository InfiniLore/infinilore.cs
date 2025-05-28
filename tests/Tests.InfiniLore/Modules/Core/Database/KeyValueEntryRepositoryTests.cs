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

namespace Tests.InfiniLore.Modules.Core.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class KeyValueEntryRepositoryTests {
    [ClassDataSource<ServiceProviderDataSource>(Shared = SharedType.PerTestSession)]
    public required ServiceProviderDataSource ServiceProvider { get; init; }

    private IUnitOfWorkFactory Infrastructure => ServiceProvider.GetRequiredService<IUnitOfWorkFactory>();
    private KeyValueEntryFaker Faker => ServiceProvider.GetRequiredService<KeyValueEntryFaker>();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Test Methods
    // -----------------------------------------------------------------------------------------------------------------
    [Test]
    public async Task BoundToCorrectRepository() {
        // Arrange
        await using IUnitOfWork unitOfWork = Infrastructure.Create();

        // Act
        var repo = await unitOfWork.GetRepositoryAsync<IKeyValueEntryRepository>();

        // Assert
        await Assert.That(repo).IsTypeOf<KeyValueEntryRepository>();
    }

    [Test]
    public async Task TryAddOrUpdateAsync_ReturnsExpectedResult() {
        // Arrange
        await using IUnitOfWork unitOfWork = Infrastructure.Create();
        var repo = await unitOfWork.GetRepositoryAsync<IKeyValueEntryRepository>();
        var dbContext = await unitOfWork.GetDbContextAsync<ContentDb>();

        KeyValueEntryModel model = Faker.Faker.Generate();
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
