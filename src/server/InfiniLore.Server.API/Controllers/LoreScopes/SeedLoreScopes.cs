// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Database;
using InfiniLore.Server.Database.Models.Account;
using InfiniLore.Server.Database.Models.UserData;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InfiniLore.Server.API.Controllers.LoreScopes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class SeedLoreScopes(IDbContextFactory<InfiniLoreDbContext> dbContextFactory, UserManager<InfiniLoreUser> userManager) : EndpointWithoutRequest {
    public override void Configure() {
        Get("/api/lore-scopes/seed");
        AllowAnonymous();
    }

    public async override Task HandleAsync(CancellationToken ct) {
        await using InfiniLoreDbContext dbContext = await dbContextFactory.CreateDbContextAsync(ct);
        
        if ( await dbContext.Users.FirstOrDefaultAsync(u => u.UserName == "testuser", cancellationToken: ct) is not {} user) return;
        dbContext.LoreScopes.Add(new LoreScopeModel {User = user});
        dbContext.LoreScopes.Add(new LoreScopeModel {User = user});
        dbContext.LoreScopes.Add(new LoreScopeModel {User = user});
        dbContext.LoreScopes.Add(new LoreScopeModel {User = user});
        dbContext.LoreScopes.Add(new LoreScopeModel {User = user});
        
        await dbContext.SaveChangesAsync(ct);
    }
}
