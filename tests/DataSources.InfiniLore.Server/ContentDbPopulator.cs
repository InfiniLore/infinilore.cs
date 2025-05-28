// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using Fakers.InfiniLore.Server;
using InfiniLore.Server.Database;
using InfiniLore.Server.Modules.Core.Database;
using InfiniLore.Server.Modules.LoreScopes.Database;
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
    public async Task MigrateAsync() {
        CancellationTokenSource cts = new ();
        cts.CancelAfter(TimeSpan.FromSeconds(5));
        CancellationToken token = cts.Token;
        
        await using ContentDb dbContext = await serviceProvider.GetRequiredService<IDbContextFactory<ContentDb>>().CreateDbContextAsync(token);
        await dbContext.Database.MigrateAsync(cancellationToken: token);
        await dbContext.SaveChangesAsync(token);
    }
    
    public async Task PopulateAsync() {
        CancellationTokenSource cts = new ();
        cts.CancelAfter(TimeSpan.FromSeconds(5));
        CancellationToken token = cts.Token;
        
        await using IUnitOfWork unitOfWork = serviceProvider.GetRequiredService<IUnitOfWorkFactory>().Create();
        var dbContext = await unitOfWork.GetDbContextAsync<ContentDb>(token);

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
                Description = null
            }
        );

        await unitOfWork.SaveChangesAsync();
    }
}
