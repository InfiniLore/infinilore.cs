// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Server.Database;
using Microsoft.Extensions.DependencyInjection;
using Tests.InfiniLore.Server.Database.Fakers;

namespace Tests.InfiniLore.Server.Database.DataSources;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ContentDbPopulator(IServiceProvider serviceProvider) {
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
