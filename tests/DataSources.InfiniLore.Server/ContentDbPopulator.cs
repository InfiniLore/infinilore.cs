// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using Fakers.InfiniLore.Server;
using InfiniLore.Server.Database;
using Microsoft.Extensions.DependencyInjection;

namespace DataSources.InfiniLore.Server;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ContentDbPopulator(IServiceProvider serviceProvider) {
    private GuidStore GuidStore { get; } = new();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async Task PopulateAsync() {
        var keyValueStoreFaker = new KeyValueStoreFaker();

        await using IUnitOfWork unitOfWork = serviceProvider.GetRequiredService<IUnitOfWorkFactory>().Create();
        var dbContext = await unitOfWork.GetDbContextAsync<ContentDb>();

        await dbContext.KeyValueStores.AddRangeAsync(
            keyValueStoreFaker.GetById(GuidStore.GetGuid(1))
        );

        await unitOfWork.SaveChangesAsync();
    }
}
