// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Data;
using InfiniLore.Server.Data.Models.UserData;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InfiniLore.Server.API.Controllers.LoreScopes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class SeedLoreScopes(IDbContextFactory<InfiniLoreDbContext> dbContextFactory, ILogger logger) : EndpointWithoutRequest {
    public override void Configure() {
        Get("/lore-scopes/seed");
        Roles("Admin");
    }

    public override async Task HandleAsync(CancellationToken ct) {
        logger.Information("User is authenticated: {IsAuthenticated}", User.Identity is { IsAuthenticated: true });
        logger.Information("User roles: {Roles}", string.Join(", ", User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value)));

        await using InfiniLoreDbContext dbContext = await dbContextFactory.CreateDbContextAsync(ct);

        if (await dbContext.Users.FirstOrDefaultAsync(predicate: u => u.UserName == "testuser", ct) is not {} user) return;

        dbContext.LoreScopes.Add(new LoreScopeModel { Owner = user, Name = "A" });
        dbContext.LoreScopes.Add(new LoreScopeModel { Owner = user, Name = "B" });
        dbContext.LoreScopes.Add(new LoreScopeModel { Owner = user, Name = "C" });
        dbContext.LoreScopes.Add(new LoreScopeModel { Owner = user, Name = "D" });
        dbContext.LoreScopes.Add(new LoreScopeModel { Owner = user, Name = "E" });

        await dbContext.SaveChangesAsync(ct);
    }
}
