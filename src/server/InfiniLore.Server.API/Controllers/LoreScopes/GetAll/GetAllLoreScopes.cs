// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts.Repositories;
using InfiniLore.Server.Contracts.Services;
using InfiniLore.Server.Data.Models.UserData;
using InfiniLoreLib;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace InfiniLore.Server.API.Controllers.LoreScopes.GetAll;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class GetAllLoreScopes(ILoreScopesRepository loreScopesRepository, IResolveUserIdService resolveUserIdService) :
    Endpoint<
        GetAllLoreScopesRequest,
        Results<
            Ok<IEnumerable<LoreScopeResponse>>,
            NotFound,
            ProblemDetails
        >,
        LoreScopeResponseMapper
    > {

    public override void Configure() {
        Get("/api/{UserId:guid}/lore-scopes/");
        AllowAnonymous();
    }

    public async override Task<Results<Ok<IEnumerable<LoreScopeResponse>>, NotFound, ProblemDetails>> ExecuteAsync(GetAllLoreScopesRequest req, CancellationToken ct) {
        if (await resolveUserIdService.ResolveUserIdAsync(req, ct) is not {} user) return TypedResults.NotFound();

        Result<IEnumerable<LoreScopeModel>> result = await loreScopesRepository.GetAllAsync(
            lorescopes => lorescopes.Where(ls => ls.UserId == user.Id),
            ct
        );

        if (result.IsFailure) {
            AddError(result.ErrorMessage ?? "Unresolved Error");
            return new ProblemDetails(ValidationFailures);
        }

        if (result.Value is null) return TypedResults.NotFound();
        if (!result.Value.Any()) return TypedResults.NotFound();

        return TypedResults.Ok(await Map.FromEntityAsync(result.Value!, ct));
    }
}
