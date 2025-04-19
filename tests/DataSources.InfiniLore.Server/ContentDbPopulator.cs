// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using Fakers.InfiniLore.Server;
using InfiniLore.Server.Database;
using InfiniLore.Server.Database.Models.Account;
using InfiniLore.Server.Database.Models.Data.Project;
using InfiniLore.Server.Database.Models.Data.User;
using Microsoft.Extensions.DependencyInjection;

namespace DataSources.InfiniLore.Server;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ContentDbPopulator(IServiceProvider serviceProvider) {
    private GuidStore GuidStore { get; } = new();
    private InfiniLoreUserFaker InfiniLoreUserFaker { get; } = new();
    private KeyValueEntryFaker KeyValueEntryFaker { get; } = new();
    private LoreScopeFaker LoreScopeFaker { get; } = new();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async Task PopulateAsync() {

        await using IUnitOfWork unitOfWork = serviceProvider.GetRequiredService<IUnitOfWorkFactory>().Create();
        var dbContext = await unitOfWork.GetDbContextAsync<ContentDb>();

        InfiniLoreUser owner = InfiniLoreUserFaker.GetById(GuidStore.GetGuid(2));
        await dbContext.Users.AddRangeAsync(
            owner
        );

        await dbContext.LoreScopes.AddRangeAsync(
            new LoreScope {
                Id = GuidStore.GetGuid("lorescope-forUser2"),
                Owner = owner,
                Name = "KNOWN NAME",
                ShortDescription = LoreScopeFaker.Generate().ShortDescription
            }
        );

        await unitOfWork.SaveChangesAsync();
    }
}
