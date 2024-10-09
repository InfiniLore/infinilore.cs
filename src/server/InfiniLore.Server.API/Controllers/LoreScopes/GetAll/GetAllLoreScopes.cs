// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Azure.Core.Diagnostics;
using InfiniLore.Server.API.Services;
using InfiniLore.Server.Contracts.Services;
using InfiniLore.Server.Data;
using InfiniLore.Server.Data.Models.Account;
using InfiniLore.Server.Data.Models.UserData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace InfiniLore.Server.API.Controllers.LoreScopes.GetAll;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class GetAllLoreScopes(IDbContextFactory<InfiniLoreDbContext> dbContextFactory, IResolveUserIdService resolveUserIdService) : 
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

        // TODO Rework how AsyncResults deliver Result objects for notifying the end-user
        if (await resolveUserIdService.ResolveUserIdAsync(req, ct) is not {} user ) {
            await SendResultAsync(TypedResults.NotFound());
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
