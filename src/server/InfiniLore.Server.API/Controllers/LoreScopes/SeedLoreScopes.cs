// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Data;
using InfiniLore.Server.Data.Models.Account;
using InfiniLore.Server.Data.Models.UserData;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InfiniLore.Server.API.Controllers.LoreScopes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class SeedLoreScopes(IDbContextFactory<InfiniLoreDbContext> dbContextFactory) : EndpointWithoutRequest {
    public override void Configure() {
        Get("/api/lore-scopes/seed");
        AllowAnonymous();
    }

    public async override Task HandleAsync(CancellationToken ct) {
        await using InfiniLoreDbContext dbContext = await dbContextFactory.CreateDbContextAsync(ct);
        
        if ( await dbContext.Users.FirstOrDefaultAsync(u => u.UserName == "testuser", cancellationToken: ct) is not {} user) return;
        dbContext.LoreScopes.Add(new LoreScopeModel {User = user, Name="A"});
        dbContext.LoreScopes.Add(new LoreScopeModel {User = user, Name="B"});
        dbContext.LoreScopes.Add(new LoreScopeModel {User = user, Name="C"});
        dbContext.LoreScopes.Add(new LoreScopeModel {User = user, Name="D"});
        dbContext.LoreScopes.Add(new LoreScopeModel {User = user, Name="E"});
        
        await dbContext.SaveChangesAsync(ct);
    }
}
