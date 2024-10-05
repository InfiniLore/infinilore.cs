// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Database;
using Microsoft.EntityFrameworkCore;

namespace InfiniLore.Server.API.Controllers.LoreScopes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class GetAllLoreScopes(IDbContextFactory<InfiniLoreDbContext> dbContextFactory) : EndpointWithoutRequest<LoreScopeDto[]> {
    public override void Configure() {
        Get("/api/lore-scopes");
        AllowAnonymous();
    }

    public async override Task HandleAsync(CancellationToken ct) {
        await using InfiniLoreDbContext dbContext = await dbContextFactory.CreateDbContextAsync(ct);

        LoreScopeDto[] data = await dbContext.LoreScopes
            .Include(l => l.User)
            .Select(loreScope => LoreScopeDto.FromModel(loreScope))
            .ToArrayAsync(ct);
        
        await SendAsync(
            data,
            cancellation: ct
        );
    }
}
