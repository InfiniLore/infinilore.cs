// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.API.Services;
using InfiniLore.Server.Database;
using InfiniLore.Server.Database.Models.Account;
using InfiniLore.Server.Database.Models.UserData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace InfiniLore.Server.API.Controllers.LoreScopes.GetAll;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class GetAllLoreScopes(IDbContextFactory<InfiniLoreDbContext> dbContextFactory, ResolveUserIdService resolveUserIdService) : 
    Endpoint<
        GetAllLoreScopesRequest, 
        Results<
            Ok<IEnumerable<LoreScopeResponse>>, 
            NotFound,
            ProblemDetails
        >, 
        LoreScopeResponseMapper
    > 
{
    
    public override void Configure() {
        Get("/api/{UserId:guid}/lore-scopes/");
        AllowAnonymous();
    }

    public async override Task HandleAsync(GetAllLoreScopesRequest req, CancellationToken ct) {
        await using InfiniLoreDbContext dbContext = await dbContextFactory.CreateDbContextAsync(ct);

        AsyncResult<InfiniLoreUser> result = await resolveUserIdService.ResolveUserIdAsync(dbContext, req, ct);
        if (result is not { Value: {} user }) {
            await SendResultAsync(result.FailedIResult!);
            return;
        }
        
        IEnumerable<LoreScopeModel> loreScopeModels = await dbContext.LoreScopes
            .AsNoTracking()
            .Include(ls => ls.Multiverses)
            .Where(ls => ls.User == user)
            .ToListAsync(ct);
            
        await SendResultAsync(TypedResults.Ok(await Map.FromEntityAsync(loreScopeModels, ct)));
    }
}
