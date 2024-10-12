// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts.Repositories;
using InfiniLore.Server.Data.Models.UserData;
using InfiniLoreLib.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace InfiniLore.Server.API.Controllers.LoreScopes.GetAll;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class GetAllLoreScopesEndpoint(IInfiniLoreUserRepository userRepository) :
    Endpoint<
        GetAllLoreScopesRequest,
        Results<
            Ok<IEnumerable<LoreScopeResponse>>,
            NotFound
        >,
        LoreScopeResponseMapper
    > {

    public override void Configure() {
        Get("/{UserId:guid}/lore-scopes/");
        AllowAnonymous();
    }

    public async override Task<Results<Ok<IEnumerable<LoreScopeResponse>>, NotFound>> ExecuteAsync(GetAllLoreScopesRequest req, CancellationToken ct) {
        ResultMany<LoreScopeModel> result = await userRepository.GetLoreScopesAsync(req.UserId, ct);
        if (result.IsFailure || result.Values is null) return TypedResults.NotFound();

        return TypedResults.Ok(result.Values.Select(ls => Map.FromEntity(ls)));
    }
}
