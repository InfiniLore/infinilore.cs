// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using Fakers.InfiniLore.Server;
using InfiniLore.Server.Database;
using InfiniLore.Server.Modules.LoreScopes.Database;
using InfiniLore.Server.Modules.MarkdownFiles.Database;
using InfiniLore.Server.Modules.Users.Database;
using Microsoft.EntityFrameworkCore;
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

        InfiniLoreUserModel owner = InfiniLoreUserFaker.GetById(GuidStore.GetGuid(2));
        
        DbSet<InfiniLoreUserModel> users = dbContext.Set<InfiniLoreUserModel>();
        await users.AddRangeAsync(
            owner
        );

        DbSet<LoreScopeModel> lorescopes = dbContext.Set<LoreScopeModel>();
        await lorescopes.AddRangeAsync(
            new LoreScopeModel {
                Id = GuidStore.GetGuid("lorescope-forUser2"),
                Owner = owner,
                OwnerId = owner.Id,
                Name = "KNOWN NAME",
                ShortDescription = LoreScopeFaker.Generate().ShortDescription
            }
        );

        DbSet<MarkdownFileModel> markdownFiles = dbContext.Set<MarkdownFileModel>();
        await markdownFiles.AddRangeAsync(
            new MarkdownFileModel {
                Id = GuidStore.GetGuid("markdownfile-lorescope-forUser2"),
                OwnerId = GuidStore.GetGuid("lorescope-forUser2"),
                Name = "TestFile.md",
                Source = "**I Am Bold**"
            }
        );

        await unitOfWork.SaveChangesAsync();
    }
}
