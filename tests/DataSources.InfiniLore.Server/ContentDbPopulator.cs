// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using Fakers.InfiniLore.Server;
using InfiniLore.Server.Database;
using InfiniLore.Server.Modules.LoreScopes.Database;
using InfiniLore.Server.Modules.MarkdownFiles.Database;
using InfiniLore.Server.Modules.Users.Database;
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
        
        var users = dbContext.Set<InfiniLoreUser>();
        await users.AddRangeAsync(
            owner
        );

        var lorescopes = dbContext.Set<LoreScope>();
        await lorescopes.AddRangeAsync(
            new LoreScope {
                Id = GuidStore.GetGuid("lorescope-forUser2"),
                Owner = owner,
                OwnerId = owner.Id,
                Name = "KNOWN NAME",
                ShortDescription = LoreScopeFaker.Generate().ShortDescription
            }
        );

        var markdownFiles = dbContext.Set<MarkdownFile>();
        await markdownFiles.AddRangeAsync(
            new MarkdownFile {
                Id = GuidStore.GetGuid("markdownfile-lorescope-forUser2"),
                OwnerId = GuidStore.GetGuid("lorescope-forUser2"),
                Name = "TestFile.md",
                Source = "**I Am Bold**"
            }
        );

        await unitOfWork.SaveChangesAsync();
    }
}
